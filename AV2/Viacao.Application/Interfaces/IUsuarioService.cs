using Viacao.Application.DTOs.Usuario;

namespace Viacao.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> CadastrarAsync(CriarUsuarioDto dto);
    Task<UsuarioResponseDto> ObterPorIdAsync(Guid id);
}