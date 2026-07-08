using Viacao.Application.DTOs.Onibus;

namespace Viacao.Application.Interfaces;

public interface IOnibusService
{
    Task<OnibusResponseDto> CadastrarAsync(CriarOnibusDto dto);
    Task AtualizarQuilometragemAsync(Guid id, AtualizarKmOnibusDto dto);
    Task RegistrarRevisaoAsync(Guid id);
    Task<IEnumerable<OnibusResponseDto>> ObterTodosAsync();
}