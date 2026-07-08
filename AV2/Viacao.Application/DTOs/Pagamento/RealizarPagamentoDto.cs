using Viacao.Domain.Enums;

namespace Viacao.Application.DTOs.Pagamento;

public record RealizarPagamentoDto(MetodoPagamento Metodo, OrigemVenda Origem, decimal ValorPago);