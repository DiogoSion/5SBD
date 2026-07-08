using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class RotaMapping : IEntityTypeConfiguration<Rota>
{
    public void Configure(EntityTypeBuilder<Rota> builder)
    {
        builder.ToTable("Rotas");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Nome)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder.Property(r => r.CidadeOrigem)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder.Property(r => r.CidadeDestino)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder.Property(r => r.DistanciaTotalKm)
            .IsRequired()
            .HasColumnType("DECIMAL(10,2)");

        builder.HasMany(r => r.Paradas)
            .WithOne()
            .HasForeignKey(p => p.RotaId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.Metadata.FindNavigation(nameof(Rota.Paradas))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}