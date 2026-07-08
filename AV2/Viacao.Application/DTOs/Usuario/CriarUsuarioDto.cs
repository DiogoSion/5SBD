using Viacao.Domain.Enums;

namespace Viacao.Application.DTOs.Usuario;

public record CriarUsuarioDto(string Nome, string Cpf, string Email, string Senha, RoleUsuario Role);