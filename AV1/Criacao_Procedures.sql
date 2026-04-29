CREATE OR REPLACE PROCEDURE sp_carregar_clientes()
LANGUAGE plpgsql
AS $$
DECLARE
    v_cur CURSOR FOR
        SELECT buyer_email, buyer_name, cpf, buyer_phone_number,
               ship_address_1, ship_address_2, ship_address_3,
               ship_city, ship_state, ship_postal_code, ship_country
        FROM carga_pedidos;
    v_rec RECORD;
BEGIN
    OPEN v_cur;
    LOOP
        FETCH v_cur INTO v_rec;
        EXIT WHEN NOT FOUND;
        IF NOT EXISTS (
            SELECT 1 FROM clientes WHERE buyer_email = v_rec.buyer_email
        ) THEN
            INSERT INTO clientes (
                buyer_email, buyer_name, cpf, buyer_phone_number,
                ship_address_1, ship_address_2, ship_address_3,
                ship_city, ship_state, ship_postal_code, ship_country
            ) VALUES (
                v_rec.buyer_email, v_rec.buyer_name, v_rec.cpf,
                v_rec.buyer_phone_number, v_rec.ship_address_1,
                v_rec.ship_address_2, v_rec.ship_address_3,
                v_rec.ship_city, v_rec.ship_state,
                v_rec.ship_postal_code, v_rec.ship_country
            );
        END IF;
    END LOOP;
    CLOSE v_cur;
END;
$$;

CREATE OR REPLACE PROCEDURE sp_carregar_produtos()
LANGUAGE plpgsql
AS $$
DECLARE
    v_cur CURSOR FOR
        SELECT sku, upc, product_name
        FROM carga_pedidos;
    v_rec RECORD;
BEGIN
    OPEN v_cur;
    LOOP
        FETCH v_cur INTO v_rec;
        EXIT WHEN NOT FOUND;
        IF NOT EXISTS (
            SELECT 1 FROM produtos WHERE sku = v_rec.sku
        ) THEN
            INSERT INTO produtos (sku, upc, product_name, estoque_atual, quantidade_reposicao)
            VALUES (v_rec.sku, v_rec.upc, v_rec.product_name, 0, 10);
        END IF;
    END LOOP;
    CLOSE v_cur;
END;
$$;

CREATE OR REPLACE PROCEDURE sp_carregar_pedidos()
LANGUAGE plpgsql
AS $$
DECLARE
    v_cur CURSOR FOR
        SELECT
            order_id,
            buyer_email,
            MIN(purchase_date)                   AS purchase_date,
            MIN(payments_date)                   AS payments_date,
            MAX(ship_service_level)              AS ship_service_level,
            SUM(item_price * quantity_purchased) AS valor_total
        FROM carga_pedidos
        GROUP BY order_id, buyer_email;
    v_rec           RECORD;
    v_id_cliente    INTEGER;
BEGIN
    OPEN v_cur;
    LOOP
        FETCH v_cur INTO v_rec;
        EXIT WHEN NOT FOUND;

        SELECT id_cliente INTO v_id_cliente
        FROM clientes
        WHERE buyer_email = v_rec.buyer_email;

        IF NOT EXISTS (SELECT 1 FROM pedidos WHERE order_id = v_rec.order_id) THEN
            INSERT INTO pedidos (
                order_id, id_cliente, purchase_date, payments_date,
                ship_service_level, valor_total, status
            ) VALUES (
                v_rec.order_id, v_id_cliente, v_rec.purchase_date,
                v_rec.payments_date, v_rec.ship_service_level,
                v_rec.valor_total, 'pendente'
            );
        END IF;
    END LOOP;
    CLOSE v_cur;
END;
$$;

CREATE OR REPLACE PROCEDURE sp_carregar_itens_pedido()
LANGUAGE plpgsql
AS $$
DECLARE
    v_cur CURSOR FOR
        SELECT order_id, order_item_id, sku, quantity_purchased, item_price, currency
        FROM carga_pedidos;
    v_rec           RECORD;
    v_id_pedido     INTEGER;
    v_id_produto    INTEGER;
BEGIN
    OPEN v_cur;
    LOOP
        FETCH v_cur INTO v_rec;
        EXIT WHEN NOT FOUND;

        SELECT id_pedido INTO v_id_pedido
        FROM pedidos
        WHERE order_id = v_rec.order_id;

        SELECT id_produto INTO v_id_produto
        FROM produtos
        WHERE sku = v_rec.sku;

        INSERT INTO itens_pedido (
            id_pedido, id_produto, order_item_id,
            quantity_purchased, item_price, currency
        ) VALUES (
            v_id_pedido, v_id_produto, v_rec.order_item_id,
            v_rec.quantity_purchased, v_rec.item_price, v_rec.currency
        );
    END LOOP;
    CLOSE v_cur;
END;
$$;

CREATE OR REPLACE PROCEDURE sp_processar_pedidos()
LANGUAGE plpgsql
AS $$
DECLARE
    v_cur_pedidos CURSOR FOR
        SELECT id_pedido
        FROM pedidos
        WHERE status = 'pendente'
        ORDER BY valor_total DESC;

    v_cur_itens CURSOR (p_id_pedido INTEGER) FOR
        SELECT
            ip.id_produto,
            ip.quantity_purchased,
            p.estoque_atual,
            p.quantidade_reposicao
        FROM itens_pedido ip
        INNER JOIN produtos p ON p.id_produto = ip.id_produto
        WHERE ip.id_pedido = p_id_pedido;

    v_rec_pedido    RECORD;
    v_rec_item      RECORD;
    v_pode_atender  BOOLEAN;
    v_multiplos     INTEGER;
    v_qtd_comprar   INTEGER;
BEGIN
    OPEN v_cur_pedidos;
    LOOP
        FETCH v_cur_pedidos INTO v_rec_pedido;
        EXIT WHEN NOT FOUND;

        v_pode_atender := TRUE;

        OPEN v_cur_itens(v_rec_pedido.id_pedido);
        LOOP
            FETCH v_cur_itens INTO v_rec_item;
            EXIT WHEN NOT FOUND;
            IF v_rec_item.estoque_atual < v_rec_item.quantity_purchased THEN
                v_pode_atender := FALSE;
                EXIT;
            END IF;
        END LOOP;
        CLOSE v_cur_itens;

        IF v_pode_atender THEN

            OPEN v_cur_itens(v_rec_pedido.id_pedido);
            LOOP
                FETCH v_cur_itens INTO v_rec_item;
                EXIT WHEN NOT FOUND;

                INSERT INTO movimentacao_estoque (
                    id_pedido, id_produto, quantidade_pedida,
                    estoque_no_momento, quantidade_debitada
                ) VALUES (
                    v_rec_pedido.id_pedido, v_rec_item.id_produto,
                    v_rec_item.quantity_purchased,
                    v_rec_item.estoque_atual,
                    v_rec_item.quantity_purchased
                );

                UPDATE produtos
                SET estoque_atual = estoque_atual - v_rec_item.quantity_purchased
                WHERE id_produto = v_rec_item.id_produto;
            END LOOP;
            CLOSE v_cur_itens;

            UPDATE pedidos
            SET status = 'atendido'
            WHERE id_pedido = v_rec_pedido.id_pedido;

        ELSE

            OPEN v_cur_itens(v_rec_pedido.id_pedido);
            LOOP
                FETCH v_cur_itens INTO v_rec_item;
                EXIT WHEN NOT FOUND;

                INSERT INTO movimentacao_estoque (
                    id_pedido, id_produto, quantidade_pedida,
                    estoque_no_momento, quantidade_debitada
                ) VALUES (
                    v_rec_pedido.id_pedido, v_rec_item.id_produto,
                    v_rec_item.quantity_purchased,
                    v_rec_item.estoque_atual,
                    0
                );

                IF v_rec_item.estoque_atual < v_rec_item.quantity_purchased THEN
                    v_multiplos  := (v_rec_item.quantity_purchased + v_rec_item.quantidade_reposicao - 1)
                                    / v_rec_item.quantidade_reposicao;
                    v_qtd_comprar := v_multiplos * v_rec_item.quantidade_reposicao;

                    INSERT INTO compras (id_produto, id_pedido, quantidade_a_comprar, status)
                    VALUES (v_rec_item.id_produto, v_rec_pedido.id_pedido, v_qtd_comprar, 'pendente');
                END IF;
            END LOOP;
            CLOSE v_cur_itens;

            UPDATE pedidos
            SET status = 'aguardando_reposicao'
            WHERE id_pedido = v_rec_pedido.id_pedido;

        END IF;
    END LOOP;
    CLOSE v_cur_pedidos;
END;
$$;

CREATE OR REPLACE PROCEDURE sp_processar_carga()
LANGUAGE plpgsql
AS $$
BEGIN
    CALL sp_carregar_clientes();
    CALL sp_carregar_produtos();
    CALL sp_carregar_pedidos();
    CALL sp_carregar_itens_pedido();
    CALL sp_processar_pedidos();
    TRUNCATE TABLE carga_pedidos;
END;
$$;

CREATE OR REPLACE PROCEDURE sp_processar_reposicao()
LANGUAGE plpgsql
AS $$
DECLARE
    v_cur_repo CURSOR FOR
        SELECT sku, quantity_received
        FROM carga_reposicao;

    v_cur_pendentes CURSOR FOR
        SELECT id_pedido
        FROM pedidos
        WHERE status = 'aguardando_reposicao';

    v_cur_itens CURSOR (p_id_pedido INTEGER) FOR
        SELECT ip.id_produto, ip.quantity_purchased, p.estoque_atual
        FROM itens_pedido ip
        INNER JOIN produtos p ON p.id_produto = ip.id_produto
        WHERE ip.id_pedido = p_id_pedido;

    v_rec_repo      RECORD;
    v_rec_pendente  RECORD;
    v_rec_item      RECORD;
    v_id_produto    INTEGER;
    v_pode_atender  BOOLEAN;
BEGIN
    OPEN v_cur_repo;
    LOOP
        FETCH v_cur_repo INTO v_rec_repo;
        EXIT WHEN NOT FOUND;

        SELECT id_produto INTO v_id_produto
        FROM produtos
        WHERE sku = v_rec_repo.sku;

        IF v_id_produto IS NOT NULL THEN
            UPDATE produtos
            SET estoque_atual = estoque_atual + v_rec_repo.quantity_received
            WHERE id_produto = v_id_produto;

            UPDATE compras
            SET status = 'recebido'
            WHERE id_produto = v_id_produto AND status = 'pendente';
        END IF;
    END LOOP;
    CLOSE v_cur_repo;

    TRUNCATE TABLE carga_reposicao;

    OPEN v_cur_pendentes;
    LOOP
        FETCH v_cur_pendentes INTO v_rec_pendente;
        EXIT WHEN NOT FOUND;

        v_pode_atender := TRUE;

        OPEN v_cur_itens(v_rec_pendente.id_pedido);
        LOOP
            FETCH v_cur_itens INTO v_rec_item;
            EXIT WHEN NOT FOUND;
            IF v_rec_item.estoque_atual < v_rec_item.quantity_purchased THEN
                v_pode_atender := FALSE;
                EXIT;
            END IF;
        END LOOP;
        CLOSE v_cur_itens;

        IF v_pode_atender THEN
            UPDATE pedidos
            SET status = 'pendente'
            WHERE id_pedido = v_rec_pendente.id_pedido;
        END IF;
    END LOOP;
    CLOSE v_cur_pendentes;

    CALL sp_processar_pedidos();
END;
$$;