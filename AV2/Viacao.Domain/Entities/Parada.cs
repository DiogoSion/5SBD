namespace Viacao.Domain.Entities;

public class Parada
{
    public Guid Id { get; private set; }
    public Guid RotaId { get; private set; }
    public string Cidade { get; private set; }
    public int Ordem { get; private set; }
    
    public bool PermiteVenda { get; private set; }
    public bool PontoTrocaMotorista { get; private set; }
    public decimal QuilometroTrecho { get; private set; }

    protected Parada() { }

    public Parada(string cidade, int ordem, bool permiteVenda, bool pontoTrocaMotorista, decimal quilometroTrecho)
    {
        Id = Guid.NewGuid();
        Cidade = cidade;
        Ordem = ordem;
        PermiteVenda = permiteVenda;
        PontoTrocaMotorista = pontoTrocaMotorista;
        QuilometroTrecho = quilometroTrecho;
    }
}