INSERT INTO compra (codigoPedido, SKU, qtd)
SELECT s.codigoPedido, s.SKU, SUM(s.qtd)
FROM StagingPedidos s
LEFT JOIN compra c ON s.codigoPedido = c.codigoPedido AND s.SKU = c.SKU
WHERE c.codigoPedido IS NULL
GROUP BY s.codigoPedido, s.SKU;