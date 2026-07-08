using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class ParadaMapping : IEntityTypeConfiguration<Parada>
{
    public void Configure(EntityTypeBuilder<Parada> builder)
    {
        builder.ToTable("Paradas");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Cidade)
            .IsRequired()
            .HasColumnType("VARCHAR(100)");

        builder.Property(p => p.Ordem)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(p => p.PermiteVenda)
            .IsRequired()
            .HasColumnType("BIT");

        builder.Property(p => p.PontoTrocaMotorista)
            .IsRequired()
            .HasColumnType("BIT");

        builder.Property(p => p.QuilometroTrecho)
            .IsRequired()
            .HasColumnType("DECIMAL(10,2)");
    }
}