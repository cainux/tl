using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Abstractions.TranslationServices;
using TL.Pokedex.Core.Entities;
using TL.Pokedex.Core.Interfaces;

namespace TL.Pokedex.Core
{
    namespace Entities
    {
        public class PocketMonster
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Habitat { get; set; }
            public bool IsLegendary { get; set; }
        }
    }

    namespace Abstractions
    {
        namespace Repositories
        {
            public interface IPocketMonsterRepository
            {
                Task<PocketMonster> GetAsync(string name);
            }
        }

        namespace TranslationServices
        {
            public interface ITranslator
            {
                Task<string> GetTranslationAsync(string source);
            }

            public interface IYoda : ITranslator { }
            public interface IWilliamShakespeare : ITranslator { }
        }
    }

    namespace Interfaces
    {
        public interface IPocketMonsterService
        {
            Task<PocketMonster> GetAsync(string name);
            Task<PocketMonster> GetTranslatedAsync(string name);
        }
    }

    namespace Services
    {
        public class PocketMonsterService : IPocketMonsterService
        {
            private readonly IPocketMonsterRepository _repository;
            private readonly IYoda _yoda;
            private readonly IWilliamShakespeare _williamShakespeare;
            private ILogger<PocketMonsterService> _logger;

            public PocketMonsterService(IPocketMonsterRepository repository, IYoda yoda, IWilliamShakespeare williamShakespeare, ILogger<PocketMonsterService> logger)
            {
                _repository = repository;
                _yoda = yoda;
                _williamShakespeare = williamShakespeare;
                _logger = logger;
            }

            public async Task<PocketMonster> GetAsync(string name)
            {
                throw new NotImplementedException();
            }

            public async Task<PocketMonster> GetTranslatedAsync(string name)
            {
                throw new NotImplementedException();
            }
        }
    }
}
