using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Viacao.Application.DTOs.Motorista;
using Viacao.Application.Interfaces;

namespace Viacao.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class MotoristasController : ControllerBase
{
    private readonly IMotoristaService _motoristaService;

    public MotoristasController(IMotoristaService motoristaService)
    {
        _motoristaService = motoristaService;
    }

    [HttpGet("disponiveis")]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> ObterDisponiveis()
    {
        var motoristas = await _motoristaService.ObterDisponiveisAsync();
        return Ok(motoristas);
    }

    [HttpPost]
    [Authorize(Roles = "FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> Cadastrar([FromBody] CriarMotoristaDto dto)
    {
        var motorista = await _motoristaService.CadastrarAsync(dto);
        return Created("", motorista);
    }

    [HttpPost("{id}/jornadas/iniciar")]
    [Authorize(Roles = "MOTORISTA,FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> IniciarJornada(Guid id)
    {
        await _motoristaService.IniciarJornadaAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/jornadas/finalizar")]
    [Authorize(Roles = "MOTORISTA,FUNCIONARIO_GUICHE")]
    public async Task<IActionResult> FinalizarJornada(Guid id, [FromBody] FinalizarJornadaDto dto)
    {
        await _motoristaService.FinalizarJornadaAsync(id, dto);
        return NoContent();
    }
}