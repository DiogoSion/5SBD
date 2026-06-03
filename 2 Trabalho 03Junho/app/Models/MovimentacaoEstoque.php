<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

class MovimentacaoEstoque extends Model
{
    protected $table = 'movimentacao_estoque';
    protected $primaryKey = 'id_movimentacao';
    public $timestamps = false;
    protected $guarded = [];
}