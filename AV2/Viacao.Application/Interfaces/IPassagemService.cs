using Viacao.Application.DTOs.Passagem;
using Viacao.Application.DTOs.Pagamento;

namespace Viacao.Application.Interfaces;

public interface IPassagemService
{
    Task<CotacaoResponseDto> CotarAsync(CotacaoRequestDto dto);
    Task<PassagemResponseDto> ComprarAsync(ComprarPassagemDto dto);
    Task ProcessarPagamentoAsync(Guid passagemId, RealizarPagamentoDto dto);
}