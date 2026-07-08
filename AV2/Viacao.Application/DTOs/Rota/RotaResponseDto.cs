namespace Viacao.Application.DTOs.Rota;

public record RotaResponseDto(Guid Id, string Nome, string CidadeOrigem, string CidadeDestino, decimal DistanciaTotalKm, IEnumerable<ParadaResponseDto> Paradas);