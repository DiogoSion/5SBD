using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Viacao.Application.DTOs.Usuario;
using Viacao.Application.Interfaces;
using Viacao.Domain.Interfaces;

namespace Viacao.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly IUsuarioRepository _usuarioRepository;

    public AuthController(IUsuarioService usuarioService, IUsuarioRepository usuarioRepository)
    {
        _usuarioService = usuarioService;
        _usuarioRepository = usuarioRepository;
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> Registrar([FromBody] CriarUsuarioDto dto)
    {
        var usuario = await _usuarioService.CadastrarAsync(dto);
        return Created("", usuario);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var usuario = await _usuarioRepository.ObterPorEmailOuCpfAsync(dto.EmailOuCpf);
        
        if (usuario == null || usuario.SenhaHash != $"hash_simulado_{dto.Senha}")
            return Unauthorized(new { message = "Credenciais inválidas." });

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("ChaveSuperSecretaViacaoApiDDD2026ParaTestesLocais");
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}

public record LoginDto(string EmailOuCpf, string Senha);