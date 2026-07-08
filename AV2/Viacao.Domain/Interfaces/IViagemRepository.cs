using Viacao.Domain.Entities;

namespace Viacao.Domain.Interfaces;

public interface IViagemRepository
{
    Task AdicionarAsync(Viagem viagem);
    Task AtualizarAsync(Viagem viagem);
    Task<Viagem?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Viagem>> ObterViagensComDetalhesAsync();
}