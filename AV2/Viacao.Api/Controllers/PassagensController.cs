using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viacao.Application.DTOs.Pagamento;
using Viacao.Application.DTOs.Passagem;
using Viacao.Application.Interfaces;

namespace Viacao.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class PassagensController : ControllerBase
{
    private readonly IPassagemService _passagemService;

    public PassagensController(IPassagemService passagemService)
    {
        _passagemService = passagemService;
    }

    [HttpGet("cotacao")]
    [AllowAnonymous]
    public async Task<IActionResult> CotarPassagem([FromQuery] CotacaoRequestDto dto)
    {
        var cotacao = await _passagemService.CotarAsync(dto);
        return Ok(cotacao);
    }

    [HttpPost]
    [Authorize(Roles = "PASSAGEIRO,FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> ComprarPassagem([FromBody] ComprarPassagemDto dto)
    {
        var passagem = await _passagemService.ComprarAsync(dto);
        return Created("", passagem);
    }

    [HttpPost("{id}/pagamentos")]
    [Authorize(Roles = "PASSAGEIRO,FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> ProcessarPagamento(Guid id, [FromBody] RealizarPagamentoDto dto)
    {
        await _passagemService.ProcessarPagamentoAsync(id, dto);
        return NoContent();
    }
}