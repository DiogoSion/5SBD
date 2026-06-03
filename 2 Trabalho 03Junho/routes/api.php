<?php

use Illuminate\Support\Facades\Route;
use App\Http\Controllers\ImportacaoController;

Route::post('/importacao/marketplace', [ImportacaoController::class, 'importarMarketplace']);