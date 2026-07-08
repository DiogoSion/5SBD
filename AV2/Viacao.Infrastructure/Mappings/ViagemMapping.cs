using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class ViagemMapping : IEntityTypeConfiguration<Viagem>
{
    public void Configure(EntityTypeBuilder<Viagem> builder)
    {
        builder.ToTable("Viagens");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.RotaId).IsRequired();
        builder.Property(v => v.OnibusId).IsRequired();
        builder.Property(v => v.MotoristaId).IsRequired(false);
        builder.Property(v => v.DataPartida).IsRequired().HasColumnType("DATETIME2");

        builder.HasMany(v => v.Passagens)
            .WithOne()
            .HasForeignKey(p => p.ViagemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Metadata.FindNavigation(nameof(Viagem.Passagens))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}