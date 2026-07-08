using Viacao.Domain.Enums;

namespace Viacao.Domain.Entities;

public class Onibus
{
    public Guid Id { get; private set; }
    public string Placa { get; private set; }
    public TipoOnibus Tipo { get; private set; }
    public int Capacidade { get; private set; }
    public decimal QuilometragemAtual { get; private set; }
    public decimal QuilometragemUltimaRevisao { get; private set; }
    public StatusOnibus Status { get; private set; }

    protected Onibus() { }
    public Onibus(string placa, TipoOnibus tipo, decimal quilometragemAtual = 0)
    {
        Id = Guid.NewGuid();
        Placa = placa;
        Tipo = tipo;
        QuilometragemAtual = quilometragemAtual;
        QuilometragemUltimaRevisao = quilometragemAtual;
        Status = StatusOnibus.ATIVO;
        DefinirCapacidade();
    }

    private void DefinirCapacidade()
    {
        Capacidade = Tipo switch
        {
            TipoOnibus.EXECUTIVO => 23,
            TipoOnibus.SEMI_LEITO => 28,
            TipoOnibus.LEITO => 32,
            _ => throw new ArgumentException("Tipo de ônibus inválido.")
        };
    }

    public void AtualizarQuilometragem(decimal kmAdicionais)
    {
        if (kmAdicionais <= 0)
            throw new ArgumentException("A quilometragem adicional deve ser maior que zero.");

        QuilometragemAtual += kmAdicionais;

        decimal kmRodadosDesdeRevisao = QuilometragemAtual - QuilometragemUltimaRevisao;
        
        if (kmRodadosDesdeRevisao >= 10000)
        {
            Status = StatusOnibus.REVISAO_PENDENTE;
        }
    }

    public void RegistrarRevisao()
    {
        QuilometragemUltimaRevisao = QuilometragemAtual;
        Status = StatusOnibus.ATIVO;
    }
}