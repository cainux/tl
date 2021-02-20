using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Abstractions.TranslationServices;
using TL.Pokedex.Core.Entities;
using TL.Pokedex.Core.Interfaces;

namespace TL.Pokedex.Core.Services
{
    public class PocketMonsterService : IPocketMonsterService
    {
        private readonly IPocketMonsterRepository _repository;
        private readonly IYoda _yoda;
        private readonly IWilliamShakespeare _williamShakespeare;
        private readonly ILogger<PocketMonsterService> _logger;

        public PocketMonsterService(IPocketMonsterRepository repository, IYoda yoda, IWilliamShakespeare williamShakespeare, ILogger<PocketMonsterService> logger)
        {
            _repository = repository;
            _yoda = yoda;
            _williamShakespeare = williamShakespeare;
            _logger = logger;
        }

        public async Task<PocketMonster> GetAsync(string name)
        {
            return await _repository.GetAsync(name);
        }

        public async Task<PocketMonster> GetTranslatedAsync(string name)
        {
            var monster = await _repository.GetAsync(name);

            if (monster == null)
            {
                return null;
            }

            try
            {
                if (string.Equals(monster.Habitat, "cave", StringComparison.InvariantCultureIgnoreCase)
                    || monster.IsLegendary)
                {
                    monster.Description = await _yoda.GetTranslationAsync(monster.Description);
                }
                else
                {
                    monster.Description = await _williamShakespeare.GetTranslationAsync(monster.Description);
                }
            }
            catch (TranslationException e)
            {
                _logger.LogWarning(e, "Error occurred when getting translation for {Pokémon}", name);
            }

            return monster;
        }
    }
}
