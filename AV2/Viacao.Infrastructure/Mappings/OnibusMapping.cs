using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class OnibusMapping : IEntityTypeConfiguration<Onibus>
{
    public void Configure(EntityTypeBuilder<Onibus> builder)
    {
        builder.ToTable("Onibus");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Placa)
            .IsRequired()
            .HasColumnType("VARCHAR(8)");

        builder.Property(o => o.Tipo)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(o => o.Capacidade)
            .IsRequired()
            .HasColumnType("INT");

        builder.Property(o => o.QuilometragemAtual)
            .IsRequired()
            .HasColumnType("DECIMAL(10,2)");

        builder.Property(o => o.QuilometragemUltimaRevisao)
            .IsRequired()
            .HasColumnType("DECIMAL(10,2)");
    }
}