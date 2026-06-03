<?php

namespace App\Services;

use App\Models\Cliente;
use App\Models\Produto;
use App\Models\Pedido;
use App\Models\ItemPedido;
use App\Models\MovimentacaoEstoque;
use App\Models\Compra;
use Illuminate\Support\Facades\DB;

class ProcessadorDePedidosService
{
    public function processarLinha(array $dados): void
    {
        DB::transaction(function () use ($dados) {
            $cliente = Cliente::firstOrCreate(
                ['buyer_email' => $dados[4]],
                [
                    'buyer_name' => $dados[5],
                    'cpf' => $dados[6],
                    'buyer_phone_number' => $dados[7],
                    'ship_address_1' => $dados[15],
                    'ship_address_2' => $dados[16],
                    'ship_address_3' => $dados[17],
                    'ship_city' => $dados[18],
                    'ship_state' => $dados[19],
                    'ship_postal_code' => $dados[20],
                    'ship_country' => $dados[21]
                ]
            );

            $produto = Produto::firstOrCreate(
                ['sku' => $dados[8]],
                [
                    'upc' => $dados[9],
                    'product_name' => $dados[10],
                    'estoque_atual' => 0,
                    'quantidade_reposicao' => 10
                ]
            );

            $pedido = Pedido::firstOrCreate(
                ['order_id' => $dados[0]],
                [
                    'id_cliente' => $cliente->id_cliente,
                    'purchase_date' => $dados[2],
                    'payments_date' => $dados[3],
                    'ship_service_level' => $dados[14],
                    'status' => 'pendente',
                    'valor_total' => 0 
                ]
            );

            $quantidadeComprada = (int)$dados[11];
            $precoItem = (float)$dados[13];

            ItemPedido::create([
                'id_pedido' => $pedido->id_pedido,
                'id_produto' => $produto->id_produto,
                'order_item_id' => $dados[1],
                'quantity_purchased' => $quantidadeComprada,
                'item_price' => $precoItem,
                'currency' => $dados[12]
            ]);

            $pedido->increment('valor_total', $quantidadeComprada * $precoItem);

            $this->processarEstoque($pedido, $produto, $quantidadeComprada);
        });
    }

    private function processarEstoque(Pedido $pedido, Produto $produto, int $quantidadeComprada): void
    {
        if ($produto->estoque_atual >= $quantidadeComprada) {
            MovimentacaoEstoque::create([
                'id_pedido' => $pedido->id_pedido,
                'id_produto' => $produto->id_produto,
                'quantidade_pedida' => $quantidadeComprada,
                'estoque_no_momento' => $produto->estoque_atual,
                'quantidade_debitada' => $quantidadeComprada
            ]);

            $produto->decrement('estoque_atual', $quantidadeComprada);
            $pedido->update(['status' => 'atendido']);
        } else {
            MovimentacaoEstoque::create([
                'id_pedido' => $pedido->id_pedido,
                'id_produto' => $produto->id_produto,
                'quantidade_pedida' => $quantidadeComprada,
                'estoque_no_momento' => $produto->estoque_atual,
                'quantidade_debitada' => 0
            ]);

            $multiplos = ceil(($quantidadeComprada - $produto->estoque_atual) / $produto->quantidade_reposicao);
            if ($multiplos <= 0) {
                $multiplos = 1; 
            }
            
            $quantidadeComprar = $multiplos * $produto->quantidade_reposicao;

            Compra::create([
                'id_produto' => $produto->id_produto,
                'id_pedido' => $pedido->id_pedido,
                'quantidade_a_comprar' => $quantidadeComprar,
                'status' => 'pendente'
            ]);

            $pedido->update(['status' => 'aguardando_reposicao']);
        }
    }
}