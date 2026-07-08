using Viacao.Application.DTOs.Pagamento;
using Viacao.Application.DTOs.Passagem;
using Viacao.Application.Interfaces;
using Viacao.Domain.Entities;
using Viacao.Domain.Enums;
using Viacao.Domain.Interfaces;
using System.Linq;

namespace Viacao.Application.Services;

public class PassagemService : IPassagemService
{
    private readonly IPassagemRepository _passagemRepository;
    private readonly IViagemRepository _viagemRepository;
    private readonly IRotaRepository _rotaRepository;
    private readonly IOnibusRepository _onibusRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PassagemService(IPassagemRepository passagemRepository, IViagemRepository viagemRepository, IRotaRepository rotaRepository, IOnibusRepository onibusRepository, IUnitOfWork unitOfWork)
    {
        _passagemRepository = passagemRepository;
        _viagemRepository = viagemRepository;
        _rotaRepository = rotaRepository;
        _onibusRepository = onibusRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CotacaoResponseDto> CotarAsync(CotacaoRequestDto dto)
    {
        var (viagem, rota, onibus, paradaOrigem, paradaDestino) = await CarregarContextoViagem(dto.ViagemId, dto.ParadaOrigemId, dto.ParadaDestinoId);

        decimal distanciaTrecho = Math.Abs(paradaDestino.QuilometroTrecho - paradaOrigem.QuilometroTrecho);
        
        // Preço base
        decimal taxaPorKm = onibus.Tipo switch
        {
            TipoOnibus.EXECUTIVO => 0.30m,
            TipoOnibus.SEMI_LEITO => 0.45m,
            TipoOnibus.LEITO => 0.60m,
            _ => 0.30m
        };

        decimal valorBase = distanciaTrecho * taxaPorKm;

        var passagemSimulada = new Passagem(dto.ViagemId, Guid.Empty, dto.ParadaOrigemId, dto.ParadaDestinoId, 0, valorBase, viagem.DataPartida);

        bool percursoCompleto = paradaOrigem.Ordem == rota.Paradas.Min(p => p.Ordem) && paradaDestino.Ordem == rota.Paradas.Max(p => p.Ordem);
        
        if (percursoCompleto)
            passagemSimulada.AplicarDescontoPercursoCompleto(10);
            // 10% de desconto

        return new CotacaoResponseDto(valorBase, passagemSimulada.ValorFinal, percursoCompleto);
    }

    public async Task<PassagemResponseDto> ComprarAsync(ComprarPassagemDto dto)
    {
        var (viagem, rota, onibus, paradaOrigem, paradaDestino) = await CarregarContextoViagem(dto.ViagemId, dto.ParadaOrigemId, dto.ParadaDestinoId);
        
        var passagensExistentes = await _passagemRepository.ObterPorViagemAsync(viagem.Id);

        var assentosOcupadosNoTrecho = passagensExistentes.Where(p => 
        {
            var pOrigem = rota.Paradas.First(r => r.Id == p.ParadaOrigemId);
            var pDestino = rota.Paradas.First(r => r.Id == p.ParadaDestinoId);

            return paradaOrigem.Ordem < pDestino.Ordem && paradaDestino.Ordem > pOrigem.Ordem;
        }).Select(p => p.NumeroAssento).ToHashSet();

        int assentoLivre = 0;
        for (int i = 1; i <= onibus.Capacidade; i++)
        {
            if (!assentosOcupadosNoTrecho.Contains(i))
            {
                assentoLivre = i;
                break;
            }
        }
        
        if (assentoLivre == 0)
            throw new InvalidOperationException("Não há assentos disponíveis para este trecho.");

        // Recalcular o valor para efetivar a compra
        var cotacao = await CotarAsync(new CotacaoRequestDto(dto.ViagemId, dto.ParadaOrigemId, dto.ParadaDestinoId, DateTime.UtcNow));

        var novaPassagem = new Passagem(dto.ViagemId, dto.PassageiroId, dto.ParadaOrigemId, dto.ParadaDestinoId, assentoLivre, cotacao.ValorBase, viagem.DataPartida);

        if (cotacao.DescontoPercursoCompletoAplicado)
            novaPassagem.AplicarDescontoPercursoCompleto(10);

        viagem.AdicionarPassagem(novaPassagem);
        await _passagemRepository.AdicionarAsync(novaPassagem);
        await _unitOfWork.CommitAsync();

        return new PassagemResponseDto(novaPassagem.Id, novaPassagem.NumeroAssento, novaPassagem.ValorFinal);
    }

    public async Task ProcessarPagamentoAsync(Guid passagemId, RealizarPagamentoDto dto)
    {
        var passagem = await _passagemRepository.ObterPorIdAsync(passagemId) 
            ?? throw new KeyNotFoundException("Passagem não encontrada.");
        
        var pagamento = new Pagamento(passagem.Id, dto.Metodo, dto.Origem, dto.ValorPago);
        passagem.RegistrarPagamento(pagamento);

        await _unitOfWork.CommitAsync();
    }

    // Evitar duplicação de busca
    private async Task<(Viagem, Rota, Onibus, Parada, Parada)> CarregarContextoViagem(Guid viagemId, Guid paradaOrigemId, Guid paradaDestinoId)
    {
        var viagem = await _viagemRepository.ObterPorIdAsync(viagemId) ?? throw new KeyNotFoundException("Viagem não encontrada.");
        var rota = await _rotaRepository.ObterPorIdAsync(viagem.RotaId) ?? throw new KeyNotFoundException("Rota não encontrada.");
        var onibus = await _onibusRepository.ObterPorIdAsync(viagem.OnibusId) ?? throw new KeyNotFoundException("Ônibus não encontrado.");

        var origem = System.Linq.Enumerable.FirstOrDefault(rota.Paradas, p => p.Id == paradaOrigemId) ?? throw new ArgumentException("Parada de origem inválida.");
        var destino = System.Linq.Enumerable.FirstOrDefault(rota.Paradas, p => p.Id == paradaDestinoId) ?? throw new ArgumentException("Parada de destino inválida.");

        if (origem.Ordem >= destino.Ordem)
            throw new ArgumentException("A ordem da parada de origem deve ser anterior à de destino.");

        return (viagem, rota, onibus, origem, destino);
    }
}