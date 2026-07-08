namespace Viacao.Domain.Entities;

public class Rota
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string CidadeOrigem { get; private set; }
    public string CidadeDestino { get; private set; }
    public decimal DistanciaTotalKm { get; private set; }
    
    private readonly List<Parada> _paradas = new();
    public IReadOnlyCollection<Parada> Paradas => _paradas.AsReadOnly();

    protected Rota() { }

    public Rota(string nome, string cidadeOrigem, string cidadeDestino, decimal distanciaTotalKm)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        CidadeOrigem = cidadeOrigem;
        CidadeDestino = cidadeDestino;
        DistanciaTotalKm = distanciaTotalKm;
    }

    public void AdicionarParada(Parada parada)
    {
        if (_paradas.Any(p => p.Ordem == parada.Ordem))
            throw new ArgumentException("Já existe uma parada com esta ordem na rota.");

        _paradas.Add(parada);
    }
}