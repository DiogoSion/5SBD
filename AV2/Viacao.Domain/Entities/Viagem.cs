namespace Viacao.Domain.Entities;

public class Viagem
{
    public Guid Id { get; private set; }
    public Guid RotaId { get; private set; }
    public Guid OnibusId { get; private set; }
    public Guid? MotoristaId { get; private set; }
    public DateTime DataPartida { get; private set; }
    
    private readonly List<Passagem> _passagens = new();
    public IReadOnlyCollection<Passagem> Passagens => _passagens.AsReadOnly();

    protected Viagem() { }

    public Viagem(Guid rotaId, Guid onibusId, DateTime dataPartida)
    {
        Id = Guid.NewGuid();
        RotaId = rotaId;
        OnibusId = onibusId;
        DataPartida = dataPartida;
    }

    public void AlocarMotorista(Motorista motorista, decimal kmTrecho, decimal horasEstimadas)
    {
        if (!motorista.PodeAssumirTrecho(kmTrecho, horasEstimadas, DataPartida))
            throw new InvalidOperationException("O motorista não cumpre os requisitos de jornada ou descanso para esta viagem.");

        MotoristaId = motorista.Id;
    }

    public void AdicionarPassagem(Passagem passagem)
    {
        _passagens.Add(passagem);
    }
}