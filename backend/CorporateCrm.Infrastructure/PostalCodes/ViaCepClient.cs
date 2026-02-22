using System.Net;
using System.Net.Http.Json;
using CorporateCrm.Application.Common.Abstractions;
using Microsoft.Extensions.Logging;

namespace CorporateCrm.Infrastructure.PostalCodes;

public sealed class ViaCepClient : IPostalCodeLookup
{
    private readonly HttpClient _http;
    private readonly ILogger<ViaCepClient> _logger;

    public ViaCepClient(HttpClient http, ILogger<ViaCepClient> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<PostalCodeLookupResult?> LookupAsync(string cep, CancellationToken ct)
    {
        var url = $"/ws/{cep}/json/";

        try
        {
            var response = await _http.GetAsync(url, ct);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var dto = await response.Content.ReadFromJsonAsync<ViaCepResponse>(cancellationToken: ct);

            if (dto is null)
                return null;

            if (IsErro(dto.Erro))
                return null;

            return new PostalCodeLookupResult(
                Cep: dto.Cep ?? cep,
                Street: dto.Logradouro,
                Neighborhood: dto.Bairro,
                City: dto.Localidade,
                State: dto.Uf
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ViaCEP lookup failed for CEP {Cep}", cep);

            throw;
        }
    }

    private sealed class ViaCepResponse
    {
        public string? Cep { get; set; }
        public string? Logradouro { get; set; }
        public string? Bairro { get; set; }
        public string? Localidade { get; set; }
        public string? Uf { get; set; }
        public object? Erro { get; set; }
    }

    private static bool IsErro(object? erro)
    {
        if (erro is null) return false;

        if (erro is bool b) return b;

        if (erro is string s)
            return string.Equals(s, "true", StringComparison.OrdinalIgnoreCase);

        return false;
    }
}