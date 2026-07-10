using Viacao.Application.DTOs.Rota;
using Viacao.Application.Interfaces;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;

namespace Viacao.Application.Services;

public class RotaService : IRotaService
{
    private readonly IRotaRepository _rotaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RotaService(IRotaRepository rotaRepository, IUnitOfWork unitOfWork)
    {
        _rotaRepository = rotaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RotaResponseDto> CadastrarRotaAsync(CriarRotaDto dto)
    {
        var rota = new Rota(dto.Nome, dto.CidadeOrigem, dto.CidadeDestino, dto.DistanciaTotalKm);

        await _rotaRepository.AdicionarAsync(rota);
        await _unitOfWork.CommitAsync();

        return MapearParaResponse(rota);
    }

    public async Task AdicionarParadaAsync(Guid rotaId, AdicionarParadaDto dto)
    {
        var rota = await _rotaRepository.ObterPorIdAsync(rotaId)
            ?? throw new KeyNotFoundException("Rota não encontrada.");

        var parada = new Parada(dto.Cidade, dto.Ordem, dto.PermiteVenda, dto.PontoTrocaMotorista, dto.QuilometroTrecho);
        
        rota.AdicionarParada(parada);
        _rotaRepository.AdicionarParada(parada);

        await _unitOfWork.CommitAsync();
    }

    public async Task<RotaResponseDto> ObterPorIdAsync(Guid id)
    {
        var rota = await _rotaRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Rota não encontrada.");

        return MapearParaResponse(rota);
    }

    public async Task<IEnumerable<RotaResponseDto>> ObterTodasAsync()
    {
        var rotas = await _rotaRepository.ObterTodasAsync();
        return rotas.Select(MapearParaResponse);
    }

    private static RotaResponseDto MapearParaResponse(Rota rota)
    {
        var paradasDto = rota.Paradas.Select(p => new ParadaResponseDto(
            p.Id, p.Cidade, p.Ordem, p.PermiteVenda, p.PontoTrocaMotorista, p.QuilometroTrecho)).ToList();

        return new RotaResponseDto(
            rota.Id, rota.Nome, rota.CidadeOrigem, rota.CidadeDestino, rota.DistanciaTotalKm, paradasDto);
    }
}