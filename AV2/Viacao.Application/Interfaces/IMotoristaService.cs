using Viacao.Application.DTOs.Motorista;

namespace Viacao.Application.Interfaces;

public interface IMotoristaService
{
    Task<MotoristaResponseDto> CadastrarAsync(CriarMotoristaDto dto);
    Task IniciarJornadaAsync(Guid id);
    Task FinalizarJornadaAsync(Guid id, FinalizarJornadaDto dto);
    Task<IEnumerable<MotoristaResponseDto>> ObterDisponiveisAsync();
    Task<IEnumerable<MotoristaResponseDto>> ObterTodosAsync();
}