using Viacao.Domain.Entities;

namespace Viacao.Domain.Interfaces;

public interface IPassagemRepository
{
    Task AdicionarAsync(Passagem passagem);
    Task<IEnumerable<Passagem>> ObterPorViagemAsync(Guid viagemId);
    Task<Passagem?> ObterPorIdAsync(Guid id);
}