using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Viacao.Domain.Entities;

namespace Viacao.Infrastructure.Mappings;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamentos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Metodo).IsRequired().HasConversion<int>();
        builder.Property(p => p.Origem).IsRequired().HasConversion<int>();
        builder.Property(p => p.ValorPago).IsRequired().HasColumnType("DECIMAL(10,2)");
        builder.Property(p => p.DataPagamento).IsRequired().HasColumnType("DATETIME2");
    }
}