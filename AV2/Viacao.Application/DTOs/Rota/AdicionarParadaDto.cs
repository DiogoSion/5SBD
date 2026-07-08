namespace Viacao.Application.DTOs.Rota;

public record AdicionarParadaDto(string Cidade, int Ordem, bool PermiteVenda, bool PontoTrocaMotorista, decimal QuilometroTrecho);