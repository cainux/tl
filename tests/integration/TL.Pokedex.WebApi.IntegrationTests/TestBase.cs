using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TL.Pokedex.WebApi.IntegrationTests
{
    public abstract class TestBase
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        protected readonly HttpClient HttpClient;

        protected TestBase(TestFixture fixture)
        {
            _jsonSerializerOptions = fixture.JsonSerializerOptions;
            HttpClient = fixture.HttpClient;
        }

        protected async Task<T> DeserializeJsonAsync<T>(Stream utf8Json)
        {
            return await JsonSerializer.DeserializeAsync<T>(utf8Json, _jsonSerializerOptions);
        }
    }
}
