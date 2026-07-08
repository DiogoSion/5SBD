using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viacao.Application.DTOs.Rota;
using Viacao.Application.Interfaces;

namespace Viacao.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class RotasController : ControllerBase
{
    private readonly IRotaService _rotaService;

    public RotasController(IRotaService rotaService)
    {
        _rotaService = rotaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObterTodas()
    {
        var rotas = await _rotaService.ObterTodasAsync();
        return Ok(rotas);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var rota = await _rotaService.ObterPorIdAsync(id);
        return Ok(rota);
    }

    [HttpPost]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> CadastrarRota([FromBody] CriarRotaDto dto)
    {
        var rota = await _rotaService.CadastrarRotaAsync(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = rota.Id }, rota);
    }

    [HttpPost("{id}/paradas")]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> AdicionarParada(Guid id, [FromBody] AdicionarParadaDto dto)
    {
        await _rotaService.AdicionarParadaAsync(id, dto);
        return NoContent();
    }
}