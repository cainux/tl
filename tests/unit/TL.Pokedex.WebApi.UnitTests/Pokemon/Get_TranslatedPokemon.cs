using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using TL.Pokedex.Core.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace TL.Pokedex.WebApi.UnitTests.Pokemon
{
    public class Get_TranslatedPokemon : IClassFixture<TestFixture>
    {
        private readonly AutoMocker _mocker;
        private readonly HttpClient SUT;

        public Get_TranslatedPokemon(TestFixture fixture, ITestOutputHelper output)
        {
            fixture.Output = output;

            _mocker = fixture.Mocker;
            SUT = fixture.WebApplicationFactory.CreateClient();
        }

        [Fact]
        public async Task Returns_OK_When_Pokemon_Exists()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPokemonService>()
                .Setup(x => x.GetTranslatedAsync(It.IsAny<string>()))
                .ReturnsAsync(new Core.Entities.Pokemon());

            // Act
            var actual = await SUT.GetAsync($"/pokemon/translated/{name}");

            // Assert
            actual.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Returns_NotFound_When_Pokemon_Does_Not_Exist()
        {
            // Arrange
            const string name = "mewtwo";

            // Act
            var actual = await SUT.GetAsync($"/pokemon/translated/{name}");

            // Assert
            actual.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
