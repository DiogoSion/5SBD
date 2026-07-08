using Viacao.Application.DTOs.Usuario;
using Viacao.Application.Interfaces;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;

namespace Viacao.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UsuarioService(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UsuarioResponseDto> CadastrarAsync(CriarUsuarioDto dto)
    {
        var usuarioExistente = await _usuarioRepository.ObterPorEmailOuCpfAsync(dto.Cpf) 
                            ?? await _usuarioRepository.ObterPorEmailOuCpfAsync(dto.Email);
        
        if (usuarioExistente != null)
            throw new InvalidOperationException("Já existe um usuário cadastrado com este CPF ou Email.");

        // Simulando o Hash.
        var senhaHash = $"hash_simulado_{dto.Senha}";

        var usuario = new Usuario(dto.Nome, dto.Cpf, dto.Email, senhaHash, dto.Role);

        await _usuarioRepository.AdicionarAsync(usuario);
        await _unitOfWork.CommitAsync();

        return MapearParaResponse(usuario);
    }

    public async Task<UsuarioResponseDto> ObterPorIdAsync(Guid id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Usuário não encontrado.");

        return MapearParaResponse(usuario);
    }

    private static UsuarioResponseDto MapearParaResponse(Usuario usuario)
    {
        return new UsuarioResponseDto(usuario.Id, usuario.Nome, usuario.Cpf, usuario.Email, usuario.Role.ToString());
    }
}