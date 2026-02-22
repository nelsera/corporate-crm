using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using CorporateCrm.Infrastructure.PostalCodes;
using CorporateCrm.Infrastructure.Tests.TestDoubles;
using Xunit;

namespace CorporateCrm.Infrastructure.Tests.PostalCodes;

public class ViaCepClientTests
{
    [Fact]
    public async Task Should_map_success_response()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(req =>
        {
            req.RequestUri!.ToString().Should().Contain("/ws/11065651/json");

            var json = """
            {
              "cep": "11065-651",
              "logradouro": "Avenida Barão de Penedo",
              "bairro": "José Menino",
              "localidade": "Santos",
              "uf": "SP"
            }
            """;

            return FakeHttpMessageHandler.Json(HttpStatusCode.OK, json);
        });

        var http = new HttpClient(handler)
        {
            BaseAddress = new System.Uri("https://viacep.com.br")
        };

        var client = new ViaCepClient(http);

        // Act
        var result = await client.LookupAsync("11065651", default);

        // Assert
        result.Should().NotBeNull();
        result!.Cep.Should().Be("11065-651");
        result.Street.Should().Be("Avenida Barão de Penedo");
        result.Neighborhood.Should().Be("José Menino");
        result.City.Should().Be("Santos");
        result.State.Should().Be("SP");
    }

    [Fact]
    public async Task Should_return_null_when_viacep_returns_erro_true()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(_ =>
        {
            var json = """{ "erro": true }""";
            return FakeHttpMessageHandler.Json(HttpStatusCode.OK, json);
        });

        var http = new HttpClient(handler)
        {
            BaseAddress = new System.Uri("https://viacep.com.br")
        };

        var client = new ViaCepClient(http);

        // Act
        var result = await client.LookupAsync("99999999", default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Should_throw_on_non_success_status_code()
    {
        // Arrange
        var handler = new FakeHttpMessageHandler(_ =>
            FakeHttpMessageHandler.Json(HttpStatusCode.InternalServerError, """{ "msg":"fail" }""")
        );

        var http = new HttpClient(handler)
        {
            BaseAddress = new System.Uri("https://viacep.com.br")
        };

        var client = new ViaCepClient(http);

        // Act
        var act = async () => await client.LookupAsync("11065651", default);

        // Assert
        await act.Should().ThrowAsync<HttpRequestException>();
    }
}