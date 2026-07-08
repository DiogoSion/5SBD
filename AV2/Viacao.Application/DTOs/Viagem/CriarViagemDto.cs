namespace Viacao.Application.DTOs.Viagem;

public record CriarViagemDto(Guid RotaId, Guid OnibusId, DateTime DataPartida);