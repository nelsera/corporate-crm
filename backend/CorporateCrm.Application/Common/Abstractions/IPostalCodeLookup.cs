namespace CorporateCrm.Application.Common.Abstractions;

public interface IPostalCodeLookup
{
    Task<PostalCodeLookupResult?> LookupAsync(string cep, CancellationToken ct);
}

public sealed record PostalCodeLookupResult(
    string Cep,
    string? Street,
    string? Neighborhood,
    string? City,
    string? State
);