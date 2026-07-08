using Microsoft.EntityFrameworkCore;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;
using Viacao.Infrastructure.Context;

namespace Viacao.Infrastructure.Repositories;

public class MotoristaRepository : IMotoristaRepository
{
    private readonly ViacaoDbContext _context;

    public MotoristaRepository(ViacaoDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Motorista motorista)
    {
        await _context.Motoristas.AddAsync(motorista);
    }

    public Task AtualizarAsync(Motorista motorista)
    {
        _context.Motoristas.Update(motorista);
        return Task.CompletedTask;
    }

    public async Task<Motorista?> ObterPorIdAsync(Guid id)
    {
        return await _context.Motoristas.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Motorista>> ObterTodosAsync()
    {
        return await _context.Motoristas.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<Motorista>> ObterDisponiveisAsync()
    {
        return await _context.Motoristas
            .Where(m => !m.EmTurno)
            .AsNoTracking()
            .ToListAsync();
    }
}