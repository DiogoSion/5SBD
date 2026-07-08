namespace Viacao.Application.DTOs.Onibus;

public record OnibusResponseDto(
    Guid Id, 
    string Placa, 
    string Tipo, 
    int Capacidade, 
    decimal QuilometragemAtual, 
    decimal QuilometragemUltimaRevisao, 
    string Status);