namespace Viacao.Application.DTOs.Passagem;

public record CotacaoRequestDto(Guid ViagemId, Guid ParadaOrigemId, Guid ParadaDestinoId, DateTime DataCompra);

public record CotacaoResponseDto(decimal ValorBase, decimal ValorFinal, bool DescontoPercursoCompletoAplicado);