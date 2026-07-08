using Viacao.Application.DTOs.Motorista;
using Viacao.Application.Interfaces;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;

namespace Viacao.Application.Services;

public class MotoristaService : IMotoristaService
{
    private readonly IMotoristaRepository _motoristaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MotoristaService(IMotoristaRepository motoristaRepository, IUnitOfWork unitOfWork)
    {
        _motoristaRepository = motoristaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<MotoristaResponseDto> CadastrarAsync(CriarMotoristaDto dto)
    {
        var motorista = new Motorista(dto.Nome, dto.Cnh);

        await _motoristaRepository.AdicionarAsync(motorista);
        await _unitOfWork.CommitAsync();

        return MapearParaResponse(motorista);
    }

    public async Task IniciarJornadaAsync(Guid id)
    {
        var motorista = await _motoristaRepository.ObterPorIdAsync(id) 
            ?? throw new KeyNotFoundException("Motorista não encontrado.");

        motorista.IniciarJornada();

        await _motoristaRepository.AtualizarAsync(motorista);
        await _unitOfWork.CommitAsync();
    }

    public async Task FinalizarJornadaAsync(Guid id, FinalizarJornadaDto dto)
    {
        var motorista = await _motoristaRepository.ObterPorIdAsync(id) 
            ?? throw new KeyNotFoundException("Motorista não encontrado.");

        motorista.RegistrarFimDeJornada(dto.KmRodados, dto.HorasTrabalhadas, dto.DataFim);

        await _motoristaRepository.AtualizarAsync(motorista);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<MotoristaResponseDto>> ObterDisponiveisAsync()
    {
        var motoristas = await _motoristaRepository.ObterDisponiveisAsync();
        return motoristas.Select(MapearParaResponse);
    }

    public async Task<IEnumerable<MotoristaResponseDto>> ObterTodosAsync()
    {
        var motoristas = await _motoristaRepository.ObterTodosAsync();
        return motoristas.Select(MapearParaResponse);
    }

    private static MotoristaResponseDto MapearParaResponse(Motorista motorista)
    {
        return new MotoristaResponseDto(
            motorista.Id,
            motorista.Nome,
            motorista.Cnh,
            motorista.HorasDirigidasNoTurno,
            motorista.KmRodadosNoTurno,
            motorista.UltimoFimDeTurno,
            motorista.EmTurno
        );
    }
}