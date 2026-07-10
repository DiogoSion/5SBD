using Viacao.Domain.Entities;

namespace Viacao.Domain.Interfaces;

public interface IRotaRepository
{
    Task AdicionarAsync(Rota rota);
    Task<Rota?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Rota>> ObterTodasAsync();
    // marcar explicitamente uma Parada nova como Added.
    void AdicionarParada(Parada parada);
}