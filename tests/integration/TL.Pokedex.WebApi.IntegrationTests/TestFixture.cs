using System;
using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace TL.Pokedex.WebApi.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        public HttpClient HttpClient { get; }
        public JsonSerializerOptions JsonSerializerOptions { get; }

        public TestFixture()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT")}.json", true)
                .AddEnvironmentVariables()
                .Build();

            var pokedexApiUri = configuration.GetValue<string>("PokedexApiUri");

            HttpClient = new HttpClient { BaseAddress = new Uri(pokedexApiUri) };

            JsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
        }
    }
}
