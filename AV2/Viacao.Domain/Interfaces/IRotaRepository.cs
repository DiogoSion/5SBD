using Viacao.Domain.Entities;

namespace Viacao.Domain.Interfaces;

public interface IRotaRepository
{
    Task AdicionarAsync(Rota rota);
    Task<Rota?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Rota>> ObterTodasAsync();
    // A Parada é salva via Cascade pelo Entity Framework ao salvar a Rota
}