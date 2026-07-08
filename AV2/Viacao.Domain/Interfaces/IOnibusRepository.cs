using Viacao.Domain.Entities;

namespace Viacao.Domain.Interfaces;

public interface IOnibusRepository
{
    Task AdicionarAsync(Onibus onibus);
    Task AtualizarAsync(Onibus onibus);
    Task<Onibus?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Onibus>> ObterTodosAsync();
    Task<bool> PlacaExisteAsync(string placa);
}