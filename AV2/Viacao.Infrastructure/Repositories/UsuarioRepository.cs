using Microsoft.EntityFrameworkCore;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;
using Viacao.Infrastructure.Context;

namespace Viacao.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ViacaoDbContext _context;

    public UsuarioRepository(ViacaoDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<Usuario?> ObterPorEmailOuCpfAsync(string identificador)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == identificador || u.Cpf == identificador);
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }
}