using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class PassagemMapping : IEntityTypeConfiguration<Passagem>
{
    public void Configure(EntityTypeBuilder<Passagem> builder)
    {
        builder.ToTable("Passagens");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PassageiroId).IsRequired();
        builder.Property(p => p.ParadaOrigemId).IsRequired();
        builder.Property(p => p.ParadaDestinoId).IsRequired();
        
        builder.Property(p => p.NumeroAssento).IsRequired().HasColumnType("INT");
        builder.Property(p => p.ValorBase).IsRequired().HasColumnType("DECIMAL(10,2)");
        builder.Property(p => p.ValorFinal).IsRequired().HasColumnType("DECIMAL(10,2)");
        builder.Property(p => p.DataCompra).IsRequired().HasColumnType("DATETIME2");

        builder.HasOne(p => p.Pagamento)
            .WithOne()
            .HasForeignKey<Pagamento>(pg => pg.PassagemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}