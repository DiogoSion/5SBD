namespace Viacao.Application.DTOs.Viagem;

public record ManifestoPassageiroDto(string NomePassageiro, int NumeroAssento, string ParadaOrigem, string ParadaDestino);

public record ManifestoResponseDto(Guid ViagemId, string PlacaOnibus, DateTime DataPartida, IEnumerable<ManifestoPassageiroDto> Passageiros);