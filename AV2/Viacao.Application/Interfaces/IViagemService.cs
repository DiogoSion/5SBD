using Viacao.Application.DTOs.Viagem;

namespace Viacao.Application.Interfaces;

public interface IViagemService
{
    Task<Guid> CadastrarAsync(CriarViagemDto dto);
    Task AlocarMotoristaAsync(Guid viagemId, AlocarMotoristaDto dto);
    Task<ManifestoResponseDto> ObterManifestoAsync(Guid viagemId);
}