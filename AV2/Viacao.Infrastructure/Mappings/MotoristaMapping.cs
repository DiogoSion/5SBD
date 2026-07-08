using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class MotoristaMapping : IEntityTypeConfiguration<Motorista>
{
    public void Configure(EntityTypeBuilder<Motorista> builder)
    {
        builder.ToTable("Motoristas");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Nome)
            .IsRequired()
            .HasColumnType("VARCHAR(150)");

        builder.Property(m => m.Cnh)
            .IsRequired()
            .HasColumnType("VARCHAR(20)");

        builder.Property(m => m.HorasDirigidasNoTurno)
            .IsRequired()
            .HasColumnType("DECIMAL(5,2)");

        builder.Property(m => m.KmRodadosNoTurno)
            .IsRequired()
            .HasColumnType("DECIMAL(10,2)");

        builder.Property(m => m.UltimoFimDeTurno)
            .IsRequired(false)
            .HasColumnType("DATETIME2");

        builder.Property(m => m.EmTurno)
            .IsRequired()
            .HasColumnType("BIT");
    }
}