using Microsoft.EntityFrameworkCore;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Context;

public class ViacaoDbContext : DbContext
{
    public ViacaoDbContext(DbContextOptions<ViacaoDbContext> options) : base(options) { }

    public DbSet<Onibus> Onibus { get; set; }
    public DbSet<Rota> Rotas { get; set; }
    public DbSet<Parada> Paradas { get; set; }
    public DbSet<Motorista> Motoristas { get; set; }
    public DbSet<Viagem> Viagens { get; set; }
    public DbSet<Passagem> Passagens { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ViacaoDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}