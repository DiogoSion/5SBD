using Viacao.Application.DTOs.Rota;

namespace Viacao.Application.Interfaces;

public interface IRotaService
{
    Task<RotaResponseDto> CadastrarRotaAsync(CriarRotaDto dto);
    Task AdicionarParadaAsync(Guid rotaId, AdicionarParadaDto dto);
    Task<RotaResponseDto> ObterPorIdAsync(Guid id);
    Task<IEnumerable<RotaResponseDto>> ObterTodasAsync();
}