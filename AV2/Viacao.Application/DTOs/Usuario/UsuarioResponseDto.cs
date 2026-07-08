namespace Viacao.Application.DTOs.Usuario;

public record UsuarioResponseDto(Guid Id, string Nome, string Cpf, string Email, string Role);