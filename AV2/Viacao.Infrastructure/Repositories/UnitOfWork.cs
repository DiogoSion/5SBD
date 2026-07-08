using Viacao.Domain.Interfaces;
using Viacao.Infrastructure.Context;

namespace Viacao.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ViacaoDbContext _context;

    public UnitOfWork(ViacaoDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> CommitAsync()
    {
        // Retorna o número de linhas mudadas no banco.
        return await _context.SaveChangesAsync() > 0;
    }
}