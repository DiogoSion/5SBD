using Viacao.Domain.Entities;

namespace Viacao.Domain.Interfaces;

public interface IMotoristaRepository
{
    Task AdicionarAsync(Motorista motorista);
    Task AtualizarAsync(Motorista motorista);
    Task<Motorista?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Motorista>> ObterTodosAsync();
    Task<IEnumerable<Motorista>> ObterDisponiveisAsync();
}