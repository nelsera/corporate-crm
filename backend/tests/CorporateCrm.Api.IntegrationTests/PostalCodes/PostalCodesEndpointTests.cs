using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

using CorporateCrm.Application.Common.Abstractions;

namespace CorporateCrm.Api.IntegrationTests.PostalCodes;

public class PostalCodesEndpointTests
{
    [Fact]
    public async Task Should_return_address_from_fake_lookup()
    {
        // Arrange
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        // Act
        var res = await client.GetAsync("/postal-codes/11065-651");

        // Assert
        res.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await res.Content.ReadFromJsonAsync<PostalCodeResponse>();
        body.Should().NotBeNull();
        body!.Cep.Should().Be("11065-651");
        body.City.Should().Be("Santos");
        body.State.Should().Be("SP");
    }

    private sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");

            builder.ConfigureServices(services =>
            {
                // remove o IPostalCodeLookup real
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IPostalCodeLookup));
                if (descriptor != null) services.Remove(descriptor);

                // add fake
                services.AddSingleton<IPostalCodeLookup>(new FakePostalCodeLookup());
            });
        }
    }

    private sealed class FakePostalCodeLookup : IPostalCodeLookup
    {
        public Task<PostalCodeLookupResult?> LookupAsync(string cep, CancellationToken ct)
        {
            return Task.FromResult<PostalCodeLookupResult?>(new PostalCodeLookupResult
            {
                Cep = "11065-651",
                Street = "Avenida Barão de Penedo",
                Neighborhood = "José Menino",
                City = "Santos",
                State = "SP"
            });
        }
    }

    private sealed class PostalCodeResponse
    {
        public string? Cep { get; set; }
        public string? Street { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }

    private sealed class PostalCodeLookupResult
    {
        public string? Cep { get; set; }
        public string? Street { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}