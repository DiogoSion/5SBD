using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Nome).IsRequired().HasColumnType("VARCHAR(150)");
        
        builder.Property(u => u.Cpf).IsRequired().HasColumnType("VARCHAR(11)");
        builder.HasIndex(u => u.Cpf).IsUnique(); // CPF único

        builder.Property(u => u.Email).IsRequired().HasColumnType("VARCHAR(150)");
        builder.HasIndex(u => u.Email).IsUnique(); // Email único

        builder.Property(u => u.SenhaHash).IsRequired().HasColumnType("VARCHAR(MAX)");
        
        builder.Property(u => u.Role).IsRequired().HasConversion<int>();
    }
}