namespace Viacao.Application.DTOs.Rota;

public record CriarRotaDto(string Nome, string CidadeOrigem, string CidadeDestino, decimal DistanciaTotalKm);