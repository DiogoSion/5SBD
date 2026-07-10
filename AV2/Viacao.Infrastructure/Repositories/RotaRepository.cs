using Microsoft.EntityFrameworkCore;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;
using Viacao.Infrastructure.Context;

namespace Viacao.Infrastructure.Repositories;

public class RotaRepository : IRotaRepository
{
    private readonly ViacaoDbContext _context;

    public RotaRepository(ViacaoDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Rota rota)
    {
        await _context.Rotas.AddAsync(rota);
    }

    public void AdicionarParada(Parada parada)
    {
        _context.Set<Parada>().Add(parada);
    }

    public async Task<Rota?> ObterPorIdAsync(Guid id)
    {
        return await _context.Rotas
            .Include(r => r.Paradas.OrderBy(p => p.Ordem))
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Rota>> ObterTodasAsync()
    {
        return await _context.Rotas.AsNoTracking().ToListAsync();
    }
}