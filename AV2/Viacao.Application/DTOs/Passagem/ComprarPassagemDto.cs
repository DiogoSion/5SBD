namespace Viacao.Application.DTOs.Passagem;

public record ComprarPassagemDto(Guid ViagemId, Guid ParadaOrigemId, Guid ParadaDestinoId, Guid PassageiroId);