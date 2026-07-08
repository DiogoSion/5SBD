using Microsoft.EntityFrameworkCore;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;
using Viacao.Infrastructure.Context;

namespace Viacao.Infrastructure.Repositories;

public class OnibusRepository : IOnibusRepository
{
    private readonly ViacaoDbContext _context;

    public OnibusRepository(ViacaoDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Onibus onibus)
    {
        await _context.Onibus.AddAsync(onibus);
    }

    public Task AtualizarAsync(Onibus onibus)
    {
        _context.Onibus.Update(onibus);
        return Task.CompletedTask;
    }

    public async Task<Onibus?> ObterPorIdAsync(Guid id)
    {
        return await _context.Onibus.FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Onibus>> ObterTodosAsync()
    {
        return await _context.Onibus.AsNoTracking().ToListAsync();
    }

    public async Task<bool> PlacaExisteAsync(string placa)
    {
        return await _context.Onibus.AnyAsync(o => o.Placa == placa);
    }
}