using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viacao.Application.DTOs.Onibus;
using Viacao.Application.Interfaces;

namespace Viacao.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class OnibusController : ControllerBase
{
    private readonly IOnibusService _onibusService;

    public OnibusController(IOnibusService onibusService)
    {
        _onibusService = onibusService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ObterTodos()
    {
        var frota = await _onibusService.ObterTodosAsync();
        return Ok(frota);
    }

    [HttpPost]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> Cadastrar([FromBody] CriarOnibusDto dto)
    {
        var onibus = await _onibusService.CadastrarAsync(dto);
        return CreatedAtAction(nameof(ObterTodos), new { id = onibus.Id }, onibus);
    }

    [HttpPatch("{id}/quilometragem")]
    [Authorize(Roles = "FUNCIONARIO_GUICHE,MOTORISTA")]
    public async Task<IActionResult> AtualizarQuilometragem(Guid id, [FromBody] AtualizarKmOnibusDto dto)
    {
        await _onibusService.AtualizarQuilometragemAsync(id, dto);
        return NoContent();
    }

    [HttpPost("{id}/revisoes")]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> RegistrarRevisao(Guid id)
    {
        await _onibusService.RegistrarRevisaoAsync(id);
        return NoContent();
    }
}