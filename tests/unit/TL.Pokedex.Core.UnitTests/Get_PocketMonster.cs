using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Entities;
using TL.Pokedex.Core.Services;
using Xunit;

namespace TL.Pokedex.Core.UnitTests
{
    public class Get_PocketMonster
    {
        private readonly AutoMocker _mocker;
        private readonly PocketMonsterService SUT;

        public Get_PocketMonster()
        {
            _mocker = new AutoMocker();
            SUT = _mocker.CreateInstance<PocketMonsterService>();
        }

        [Fact]
        public async Task Returns_PocketMonster_When_Found()
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

            // Act
            var actual = await SUT.GetAsync(name);

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
            var actual = await SUT.GetAsync(name);

            // Assert
            actual.Should().BeNull();
        }
    }
}
