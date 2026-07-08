using Microsoft.EntityFrameworkCore;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;
using Viacao.Infrastructure.Context;

namespace Viacao.Infrastructure.Repositories;

public class ViagemRepository : IViagemRepository
{
    private readonly ViacaoDbContext _context;

    public ViagemRepository(ViacaoDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Viagem viagem)
    {
        await _context.Viagens.AddAsync(viagem);
    }

    public Task AtualizarAsync(Viagem viagem)
    {
        _context.Viagens.Update(viagem);
        return Task.CompletedTask;
    }

    public async Task<Viagem?> ObterPorIdAsync(Guid id)
    {
        return await _context.Viagens.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IEnumerable<Viagem>> ObterViagensComDetalhesAsync()
    {
        return await _context.Viagens
            .Include(v => v.Passagens)
            .AsNoTracking()
            .ToListAsync();
    }
}