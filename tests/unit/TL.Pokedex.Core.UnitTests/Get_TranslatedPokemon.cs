using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Abstractions.TranslationServices;
using TL.Pokedex.Core.Entities;
using TL.Pokedex.Core.Services;
using Xunit;

namespace TL.Pokedex.Core.UnitTests
{
    public class Get_TranslatedPokemon
    {
        private readonly AutoMocker _mocker;
        private readonly PokemonService SUT;

        public Get_TranslatedPokemon()
        {
            _mocker = new AutoMocker();
            SUT = _mocker.CreateInstance<PokemonService>();

            _mocker.GetMock<ITranslationService>()
                .Setup(x => x.GetTranslationAsync("yoda", It.IsAny<string>()))
                .ReturnsAsync("description translated by Yoda");

            _mocker.GetMock<ITranslationService>()
                .Setup(x => x.GetTranslationAsync("shakespeare", It.IsAny<string>()))
                .ReturnsAsync("description translated by Shakespeare");
        }

        [Fact]
        public async Task Is_Normally_Shakespeare_Translated()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPokemonRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new Pokemon
                {
                    Name = name,
                    Description = "description"
                });

            // Act
            var actual = await SUT.GetTranslatedAsync(name);

            // Assert
            actual.Should().BeEquivalentTo(new
            {
                Name = name,
                Description = "description translated by Shakespeare"
            });
        }

        [Fact]
        public async Task Is_Yoda_Translated_When_Legendary()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPokemonRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new Pokemon
                {
                    Name = name,
                    Description = "description",
                    IsLegendary = true
                });

            // Act
            var actual = await SUT.GetTranslatedAsync(name);

            // Assert
            actual.Should().BeEquivalentTo(new
            {
                Name = name,
                Description = "description translated by Yoda",
                IsLegendary = true
            });
        }

        [Fact]
        public async Task Is_Yoda_Translated_When_Habitat_Is_Cave()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPokemonRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new Pokemon
                {
                    Name = name,
                    Description = "description",
                    Habitat = "cave"
                });

            // Act
            var actual = await SUT.GetTranslatedAsync(name);

            // Assert
            actual.Should().BeEquivalentTo(new
            {
                Name = name,
                Description = "description translated by Yoda",
                Habitat = "cave"
            });
        }

        [Fact]
        public async Task Is_Not_Translated_When_TranslationError_Occurs()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPokemonRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new Pokemon
                {
                    Name = name,
                    Description = "description",
                    Habitat = "habitat",
                    IsLegendary = false
                });

            _mocker.GetMock<ITranslationService>()
                .Setup(x => x.GetTranslationAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Throws<TranslationException>();

            // Act
            var actual = await SUT.GetTranslatedAsync(name);

            // Assert
            actual.Should().BeEquivalentTo(new
            {
                Name = name,
                Description = "description",
                Habitat = "habitat",
                IsLegendary = false
            });
        }

        [Fact]
        public async Task Returns_Pokemon_When_Name_Is_Cased_Differently()
        {
            // Arrange
            const string name = "MewtwO";

            _mocker.GetMock<IPokemonRepository>()
                .Setup(x => x.GetAsync("mewtwo"))
                .ReturnsAsync(new Pokemon
                {
                    Name = name,
                    Description = "description"
                });

            // Act
            var actual = await SUT.GetTranslatedAsync(name);

            // Assert
            actual.Should().BeEquivalentTo(new
            {
                Name = name,
                Description = "description translated by Shakespeare"
            });
        }

        [Fact]
        public async Task Returns_Null_When_Not_Found()
        {
            // Arrange
            const string name = "cain";

            // Act
            var actual = await SUT.GetTranslatedAsync(name);

            // Assert
            actual.Should().BeNull();
        }
    }
}
