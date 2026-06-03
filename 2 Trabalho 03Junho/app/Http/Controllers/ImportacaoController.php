<?php

namespace App\Http\Controllers;

use App\Http\Requests\ImportarMarketplaceRequest;
use App\Jobs\ProcessarCargaMarketplaceJob;
use OpenApi\Attributes as OA;

class ImportacaoController extends Controller
{
    #[OA\Post(
        path: '/api/importacao/marketplace',
        summary: 'Importar arquivo CSV de pedidos',
        tags: ['Importação']
    )]
    #[OA\RequestBody(
        required: true,
        content: new OA\MediaType(
            mediaType: 'multipart/form-data',
            schema: new OA\Schema(
                required: ['arquivo'],
                properties: [
                    new OA\Property(
                        property: 'arquivo',
                        type: 'string',
                        format: 'binary'
                    )
                ]
            )
        )
    )]
    #[OA\Response(response: 202, description: 'Processamento iniciado na fila.')]
    public function importarMarketplace(ImportarMarketplaceRequest $request)
    {
        $caminhoArquivo = $request->file('arquivo')->store('cargas');

        ProcessarCargaMarketplaceJob::dispatch($caminhoArquivo);

        return response()->json(['message' => 'Processamento iniciado na fila.'], 202);
    }
}