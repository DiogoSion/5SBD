namespace Viacao.Domain.Entities;

public class Passagem
{
    public Guid Id { get; private set; }
    public Guid ViagemId { get; private set; }
    public Guid PassageiroId { get; private set; }
    public Guid ParadaOrigemId { get; private set; }
    public Guid ParadaDestinoId { get; private set; }
    
    public int NumeroAssento { get; private set; }
    public decimal ValorBase { get; private set; }
    public decimal ValorFinal { get; private set; }
    public DateTime DataCompra { get; private set; }

    public Pagamento? Pagamento { get; private set; }

    protected Passagem() { }
    public Passagem(Guid viagemId, Guid passageiroId, Guid paradaOrigemId, Guid paradaDestinoId, int numeroAssento, decimal valorBase, DateTime dataDaViagem)
    {
        Id = Guid.NewGuid();
        ViagemId = viagemId;
        PassageiroId = passageiroId;
        ParadaOrigemId = paradaOrigemId;
        ParadaDestinoId = paradaDestinoId;
        NumeroAssento = numeroAssento;
        ValorBase = valorBase;
        ValorFinal = valorBase;
        DataCompra = DateTime.UtcNow;

        AplicarDescontoAntecedencia(dataDaViagem);
    }

    public void AplicarDescontoPercursoCompleto(decimal percentualDesconto)
    {
        var valorDesconto = ValorFinal * (percentualDesconto / 100);
        ValorFinal -= valorDesconto;
    }

    private void AplicarDescontoAntecedencia(DateTime dataDaViagem)
    {
        var diasAntecedencia = (dataDaViagem.Date - DataCompra.Date).TotalDays;

        if (diasAntecedencia >= 30)
        {
            // 15% de desconto para 30 dias ou mais
            ValorFinal -= ValorFinal * 0.15m;
        }
        else if (diasAntecedencia >= 15)
        {
            // 5% de desconto para 15 a 29 dias
            ValorFinal -= ValorFinal * 0.05m; 
        }
    }

    public void RegistrarPagamento(Pagamento pagamento)
    {
        if (pagamento.ValorPago < ValorFinal)
            throw new InvalidOperationException("O valor do pagamento é inferior ao valor da passagem.");

        Pagamento = pagamento;
    }
}