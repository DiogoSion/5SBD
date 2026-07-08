using Viacao.Domain.Entities;

namespace Viacao.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task AdicionarAsync(Usuario usuario);
    Task<Usuario?> ObterPorEmailOuCpfAsync(string identificador);
    Task<Usuario?> ObterPorIdAsync(Guid id);
}