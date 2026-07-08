using Viacao.Domain.Enums;

namespace Viacao.Domain.Entities;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cpf { get; private set; }
    public string Email { get; private set; }
    public string SenhaHash { get; private set; }
    public RoleUsuario Role { get; private set; }

    protected Usuario() { }

    public Usuario(string nome, string cpf, string email, string senhaHash, RoleUsuario role)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Email = email;
        SenhaHash = senhaHash;
        Role = role;
    }
}