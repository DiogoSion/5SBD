using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viacao.Application.DTOs.Viagem;
using Viacao.Application.Interfaces;

namespace Viacao.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ViagensController : ControllerBase
{
    private readonly IViagemService _viagemService;

    public ViagensController(IViagemService viagemService)
    {
        _viagemService = viagemService;
    }

    [HttpPost]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> Cadastrar([FromBody] CriarViagemDto dto)
    {
        var id = await _viagemService.CadastrarAsync(dto);
        return Created("", new { ViagemId = id });
    }

    [HttpPost("{id}/alocar-motorista")]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> AlocarMotorista(Guid id, [FromBody] AlocarMotoristaDto dto)
    {
        await _viagemService.AlocarMotoristaAsync(id, dto);
        return NoContent();
    }

    [HttpGet("{id}/manifesto")]
    [Authorize(Roles = "MOTORISTA,FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> ObterManifesto(Guid id)
    {
        var manifesto = await _viagemService.ObterManifestoAsync(id);
        return Ok(manifesto);
    }
}