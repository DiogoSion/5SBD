using Viacao.Domain.Enums;

namespace Viacao.Domain.Entities;

public class Pagamento
{
    public Guid Id { get; private set; }
    public Guid PassagemId { get; private set; }
    public MetodoPagamento Metodo { get; private set; }
    public OrigemVenda Origem { get; private set; }
    public decimal ValorPago { get; private set; }
    public DateTime DataPagamento { get; private set; }

    protected Pagamento() { }

    public Pagamento(Guid passagemId, MetodoPagamento metodo, OrigemVenda origem, decimal valorPago)
    {
        ValidarMetodoOrigem(metodo, origem);

        Id = Guid.NewGuid();
        PassagemId = passagemId;
        Metodo = metodo;
        Origem = origem;
        ValorPago = valorPago;
        DataPagamento = DateTime.UtcNow;
    }

    private void ValidarMetodoOrigem(MetodoPagamento metodo, OrigemVenda origem)
    {
        if (origem == OrigemVenda.INTERNET && metodo == MetodoPagamento.DINHEIRO)
            throw new ArgumentException("Pagamentos em dinheiro não são aceitos via Internet.");

        if (origem == OrigemVenda.INTERNET && !(metodo == MetodoPagamento.PIX || metodo == MetodoPagamento.CARTAO_CREDITO || metodo == MetodoPagamento.CARTAO_DEBITO))
            throw new ArgumentException("Método de pagamento inválido para a Internet.");
    }
}