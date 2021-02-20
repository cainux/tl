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
    public class Get_TranslatedPocketMonster
    {
        private readonly AutoMocker _mocker;
        private readonly PocketMonsterService SUT;

        public Get_TranslatedPocketMonster()
        {
            _mocker = new AutoMocker();
            SUT = _mocker.CreateInstance<PocketMonsterService>();

            _mocker.GetMock<IYoda>()
                .Setup(x => x.GetTranslationAsync(It.IsAny<string>()))
                .ReturnsAsync("description translated by Yoda");

            _mocker.GetMock<IWilliamShakespeare>()
                .Setup(x => x.GetTranslationAsync(It.IsAny<string>()))
                .ReturnsAsync("description translated by Shakespeare");
        }

        [Fact]
        public async Task Is_Normally_Shakespeare_Translated()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPocketMonsterRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new PocketMonster
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

            _mocker.GetMock<IPocketMonsterRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new PocketMonster
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

            _mocker.GetMock<IPocketMonsterRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new PocketMonster
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
        public async Task Is_Not_Translated_When_Error_Occurs()
        {
            // Arrange
            const string name = "mewtwo";

            _mocker.GetMock<IPocketMonsterRepository>()
                .Setup(x => x.GetAsync(name))
                .ReturnsAsync(new PocketMonster
                {
                    Name = name,
                    Description = "description",
                    Habitat = "habitat",
                    IsLegendary = false
                });

            _mocker.GetMock<IWilliamShakespeare>()
                .Setup(x => x.GetTranslationAsync(It.IsAny<string>()))
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
