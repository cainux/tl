using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using TL.Pokedex.WebApi.IntegrationTests.Entities;
using Xunit;

namespace TL.Pokedex.WebApi.IntegrationTests.PokemonGetting
{
    public class Get_Pokemon : TestBase, IClassFixture<TestFixture>
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        public Get_Pokemon(TestFixture fixture) : base(fixture)
        {
            _httpResponseMessage = HttpClient.GetAsync("/pokemon/mewtwo").Result;
        }

        [Fact]
        public void Http_Status_Code_Returned_Is_Ok()
        {
            _httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Pokemon_Is_Returned()
        {
            var monster = await DeserializeJsonAsync<Pokemon>(
                await _httpResponseMessage.Content.ReadAsStreamAsync()
            );

            monster.Should().NotBeNull();
            monster.Name.Should().Be("mewtwo");
            monster.Description.Should().NotBeNullOrEmpty();
            monster.Habitat.Should().NotBeNullOrEmpty();
            monster.IsLegendary.Should().BeTrue();
        }
    }
}
