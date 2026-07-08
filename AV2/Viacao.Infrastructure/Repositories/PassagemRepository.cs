using Microsoft.EntityFrameworkCore;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;
using Viacao.Infrastructure.Context;

namespace Viacao.Infrastructure.Repositories;

public class PassagemRepository : IPassagemRepository
{
    private readonly ViacaoDbContext _context;

    public PassagemRepository(ViacaoDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Passagem passagem)
    {
        await _context.Passagens.AddAsync(passagem);
    }

    public async Task<IEnumerable<Passagem>> ObterPorViagemAsync(Guid viagemId)
    {
        return await _context.Passagens
            .Where(p => p.ViagemId == viagemId)
            .Include(p => p.Pagamento)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Passagem?> ObterPorIdAsync(Guid id)
    {
        return await _context.Passagens.Include(p => p.Pagamento).FirstOrDefaultAsync(p => p.Id == id);
    }
}