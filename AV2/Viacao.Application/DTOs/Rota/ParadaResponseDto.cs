namespace Viacao.Application.DTOs.Rota;

public record ParadaResponseDto(Guid Id, string Cidade, int Ordem, bool PermiteVenda, bool PontoTrocaMotorista, decimal QuilometroTrecho);