namespace Viacao.Application.DTOs.Motorista;

public record MotoristaResponseDto(
    Guid Id, 
    string Nome, 
    string Cnh, 
    decimal HorasDirigidasNoTurno, 
    decimal KmRodadosNoTurno, 
    DateTime? UltimoFimDeTurno,
    bool EmTurno);