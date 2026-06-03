<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class ItemPedido extends Model
{
    protected $table = 'itens_pedido';
    protected $primaryKey = 'id_item';
    public $timestamps = false;
    protected $guarded = [];
}