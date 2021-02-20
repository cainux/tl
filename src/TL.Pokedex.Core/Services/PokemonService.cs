using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Abstractions.TranslationServices;
using TL.Pokedex.Core.Entities;
using TL.Pokedex.Core.Interfaces;

namespace TL.Pokedex.Core.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _repository;
        private readonly ITranslationService _translationService;
        private readonly ILogger<PokemonService> _logger;

        public PokemonService(IPokemonRepository repository, ITranslationService translationService, ILogger<PokemonService> logger)
        {
            _repository = repository;
            _translationService = translationService;
            _logger = logger;
        }

        public async Task<Pokemon> GetAsync(string name)
        {
            return await _repository.GetAsync(name);
        }

        public async Task<Pokemon> GetTranslatedAsync(string name)
        {
            var monster = await _repository.GetAsync(name);

            if (monster == null)
            {
                return null;
            }

            var translatorName = Translators.Shakespeare;

            if (string.Equals(monster.Habitat, "cave", StringComparison.InvariantCultureIgnoreCase)
                || monster.IsLegendary)
            {
                translatorName = Translators.Yoda;
            }

            try
            {
                monster.Description = await _translationService.GetTranslationAsync(translatorName, monster.Description);
            }
            catch (TranslationException e)
            {
                _logger.LogWarning(e, "Error occurred when getting translation from {TranslatorName} for {Pokemon}", translatorName, name);
            }

            return monster;
        }
    }
}
