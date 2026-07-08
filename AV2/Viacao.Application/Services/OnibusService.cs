using Viacao.Application.DTOs.Onibus;
using Viacao.Application.Interfaces;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;

namespace Viacao.Application.Services;

public class OnibusService : IOnibusService
{
    private readonly IOnibusRepository _onibusRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OnibusService(IOnibusRepository onibusRepository, IUnitOfWork unitOfWork)
    {
        _onibusRepository = onibusRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OnibusResponseDto> CadastrarAsync(CriarOnibusDto dto)
    {
        if (await _onibusRepository.PlacaExisteAsync(dto.Placa))
            throw new InvalidOperationException("Já existe um ônibus com esta placa.");

        var onibus = new Onibus(dto.Placa, dto.Tipo, dto.QuilometragemAtual);

        await _onibusRepository.AdicionarAsync(onibus);
        await _unitOfWork.CommitAsync();

        return MapearParaResponse(onibus);
    }

    public async Task AtualizarQuilometragemAsync(Guid id, AtualizarKmOnibusDto dto)
    {
        var onibus = await _onibusRepository.ObterPorIdAsync(id) 
            ?? throw new KeyNotFoundException("Ônibus não encontrado.");

        onibus.AtualizarQuilometragem(dto.KmAdicionais);

        await _onibusRepository.AtualizarAsync(onibus);
        await _unitOfWork.CommitAsync();
    }

    public async Task RegistrarRevisaoAsync(Guid id)
    {
        var onibus = await _onibusRepository.ObterPorIdAsync(id) 
            ?? throw new KeyNotFoundException("Ônibus não encontrado.");

        onibus.RegistrarRevisao();

        await _onibusRepository.AtualizarAsync(onibus);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<OnibusResponseDto>> ObterTodosAsync()
    {
        var frota = await _onibusRepository.ObterTodosAsync();
        return frota.Select(MapearParaResponse);
    }

    private static OnibusResponseDto MapearParaResponse(Onibus onibus)
    {
        return new OnibusResponseDto(
            onibus.Id,
            onibus.Placa,
            onibus.Tipo.ToString(),
            onibus.Capacidade,
            onibus.QuilometragemAtual,
            onibus.QuilometragemUltimaRevisao,
            onibus.Status.ToString()
        );
    }
}