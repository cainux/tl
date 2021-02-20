using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using TL.Pokedex.WebApi.IntegrationTests.Entities;
using Xunit;

namespace TL.Pokedex.WebApi.IntegrationTests.PokemonGetting
{
    public class Get_Pokemon : TestBase, IClassFixture<TestFixture>
    {
        public Get_Pokemon(TestFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Pokemon_Is_Returned()
        {
            var res = await HttpClient.GetAsync("/pokemon/mewtwo");

            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var monster = await DeserializeJsonAsync<Pokemon>(
                await res.Content.ReadAsStreamAsync()
            );

            monster.Should().NotBeNull();
            monster.Name.Should().Be("mewtwo");
            monster.Description.Should().NotBeNullOrEmpty();
            monster.Habitat.Should().NotBeNullOrEmpty();
            monster.IsLegendary.Should().BeTrue();
        }
    }
}
