﻿using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using TL.Pokedex.WebApi.IntegrationTests.Entities;
using Xunit;

namespace TL.Pokedex.WebApi.IntegrationTests.PokemonGetting
{
    /*
     * BEWARE OF THE RATE LIMIT! (we're only allowed 5 translations per hour)
     */
    public class Get_TranslatedPokemon : TestBase, IClassFixture<TestFixture>
    {
        public Get_TranslatedPokemon(TestFixture fixture) : base(fixture) { }

        [Fact]
        public async Task Pokemon_Is_Returned()
        {
            var res = await HttpClient.GetAsync("/pokemon/translated/ditto");

            res.StatusCode.Should().Be(HttpStatusCode.OK);

            var monster = await DeserializeJsonAsync<Pokemon>(
                await res.Content.ReadAsStreamAsync()
            );

            monster.Should().NotBeNull();
            monster.Name.Should().Be("ditto");
            monster.Description.Should().NotBeNullOrEmpty();
            monster.Habitat.Should().NotBeNullOrEmpty();
            monster.IsLegendary.Should().BeFalse();
        }
    }
}
