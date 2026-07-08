namespace Viacao.Domain.Entities;

public class Motorista
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public string Cnh { get; private set; }
    
    public decimal HorasDirigidasNoTurno { get; private set; }
    public decimal KmRodadosNoTurno { get; private set; }
    public DateTime? UltimoFimDeTurno { get; private set; }
    public bool EmTurno { get; private set; }

    protected Motorista() { }

    public Motorista(string nome, string cnh)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cnh = cnh;
        HorasDirigidasNoTurno = 0;
        KmRodadosNoTurno = 0;
        EmTurno = false;
    }

    public bool PodeAssumirTrecho(decimal kmTrechoEstimado, decimal horasEstimadas, DateTime dataEmbarque)
    {
        if (UltimoFimDeTurno.HasValue)
        {
            var horasDescanso = (dataEmbarque - UltimoFimDeTurno.Value).TotalHours;
            if (horasDescanso < 12) return false;
        }

        if (HorasDirigidasNoTurno + horasEstimadas > 6) return false;
        if (KmRodadosNoTurno + kmTrechoEstimado > 400) return false;

        return true;
    }

    public void IniciarJornada()
    {
        if (EmTurno) throw new InvalidOperationException("Motorista já está em turno.");
        EmTurno = true;
    }

    public void RegistrarFimDeJornada(decimal kmRodadosNoTrecho, decimal horasTrabalhadasNoTrecho, DateTime dataFim)
    {
        if (!EmTurno) throw new InvalidOperationException("Motorista não está em turno.");

        KmRodadosNoTurno += kmRodadosNoTrecho;
        HorasDirigidasNoTurno += horasTrabalhadasNoTrecho;
        UltimoFimDeTurno = dataFim;
        EmTurno = false;
    }

    public void ResetarContadoresDeJornada()
    {
        HorasDirigidasNoTurno = 0;
        KmRodadosNoTurno = 0;
    }
}