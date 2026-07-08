using Viacao.Domain.Enums;

namespace Viacao.Application.DTOs.Onibus;

public record CriarOnibusDto(string Placa, TipoOnibus Tipo, decimal QuilometragemAtual);