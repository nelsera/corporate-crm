using CorporateCrm.Application.Common.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CorporateCrm.Api.Controllers;

[ApiController]
[Route("postal-codes")]
public sealed class PostalCodesController : ControllerBase
{
    private readonly IPostalCodeLookup _lookup;

    public PostalCodesController(IPostalCodeLookup lookup)
    {
        _lookup = lookup;
    }

    [HttpGet("{cep}")]
    public async Task<IActionResult> GetByCep([FromRoute] string cep, CancellationToken ct)
    {
        var normalized = new string(cep.Where(char.IsDigit).ToArray());

        if (normalized.Length != 8)
            return BadRequest(new { message = "CEP must have 8 digits." });

        var result = await _lookup.LookupAsync(normalized, ct);

        if (result is null)
            return NotFound(new { message = "CEP not found." });

        return Ok(result);
    }
}