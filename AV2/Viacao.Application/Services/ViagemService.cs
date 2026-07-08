using Viacao.Application.DTOs.Viagem;
using Viacao.Application.Interfaces;
using Viacao.Domain.Entities;
using Viacao.Domain.Interfaces;

namespace Viacao.Application.Services;

public class ViagemService : IViagemService
{
    private readonly IViagemRepository _viagemRepository;
    private readonly IMotoristaRepository _motoristaRepository;
    private readonly IOnibusRepository _onibusRepository;
    private readonly IRotaRepository _rotaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ViagemService(IViagemRepository viagemRepository, IMotoristaRepository motoristaRepository, IOnibusRepository onibusRepository, IRotaRepository rotaRepository, IUnitOfWork unitOfWork)
    {
        _viagemRepository = viagemRepository;
        _motoristaRepository = motoristaRepository;
        _onibusRepository = onibusRepository;
        _rotaRepository = rotaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CadastrarAsync(CriarViagemDto dto)
    {
        var viagem = new Viagem(dto.RotaId, dto.OnibusId, dto.DataPartida);
        await _viagemRepository.AdicionarAsync(viagem);
        await _unitOfWork.CommitAsync();
        return viagem.Id;
    }

    public async Task AlocarMotoristaAsync(Guid viagemId, AlocarMotoristaDto dto)
    {
        var viagem = await _viagemRepository.ObterPorIdAsync(viagemId) ?? throw new KeyNotFoundException("Viagem não encontrada.");
        var motorista = await _motoristaRepository.ObterPorIdAsync(dto.MotoristaId) ?? throw new KeyNotFoundException("Motorista não encontrado.");

        viagem.AlocarMotorista(motorista, dto.KmTrecho, dto.HorasEstimadas);
        
        await _viagemRepository.AtualizarAsync(viagem);
        await _unitOfWork.CommitAsync();
    }

    public async Task<ManifestoResponseDto> ObterManifestoAsync(Guid viagemId)
    {
        var viagem = await _viagemRepository.ObterPorIdAsync(viagemId) ?? throw new KeyNotFoundException("Viagem não encontrada.");
        var onibus = await _onibusRepository.ObterPorIdAsync(viagem.OnibusId);
        var rota = await _rotaRepository.ObterPorIdAsync(viagem.RotaId);
        var passagens = await _viagemRepository.ObterViagensComDetalhesAsync();
        
        var viagemComPassagens = passagens.First(v => v.Id == viagemId);

        var passageiros = viagemComPassagens.Passagens.Select(p => 
        {
            var origem = rota!.Paradas.First(r => r.Id == p.ParadaOrigemId).Cidade;
            var destino = rota.Paradas.First(r => r.Id == p.ParadaDestinoId).Cidade;
            return new ManifestoPassageiroDto(p.PassageiroId.ToString(), p.NumeroAssento, origem, destino);
        }).OrderBy(p => p.NumeroAssento).ToList();

        return new ManifestoResponseDto(viagemId, onibus!.Placa, viagem.DataPartida, passageiros);
    }
}