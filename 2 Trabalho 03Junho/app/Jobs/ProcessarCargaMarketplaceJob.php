<?php

namespace App\Jobs;

use Illuminate\Bus\Queueable;
use Illuminate\Contracts\Queue\ShouldQueue;
use Illuminate\Foundation\Bus\Dispatchable;
use Illuminate\Queue\InteractsWithQueue;
use Illuminate\Queue\SerializesModels;
use Illuminate\Support\Facades\Storage;
use App\Services\ProcessadorDePedidosService;

class ProcessarCargaMarketplaceJob implements ShouldQueue
{
    use Dispatchable, InteractsWithQueue, Queueable, SerializesModels;

    public function __construct(private string $caminhoArquivo) {}

    public function handle(ProcessadorDePedidosService $servicoDePedidos): void
    {
        $caminhoAbsoluto = Storage::path($this->caminhoArquivo);
        $handle = fopen($caminhoAbsoluto, 'r');

        fgetcsv($handle);

        while (($linha = fgetcsv($handle, 4000, ',')) !== false) {
            $servicoDePedidos->processarLinha($linha);
        }

        fclose($handle);
        Storage::delete($this->caminhoArquivo);
    }
}