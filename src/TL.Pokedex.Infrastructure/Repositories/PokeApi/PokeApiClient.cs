using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TL.Pokedex.Core.Abstractions.Repositories;
using TL.Pokedex.Core.Entities;

namespace TL.Pokedex.Infrastructure.Repositories.PokeApi
{
    public class PokeApiClient : IPokemonRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PokeApiClient> _logger;

        public PokeApiClient(HttpClient httpClient, ILogger<PokeApiClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Pokemon> GetAsync(string name)
        {
            _logger.LogDebug("Calling PokeApi to get {PokemonName}", name);

            dynamic searchResult = JsonConvert.DeserializeObject<JObject>(
                await _httpClient.GetStringAsync(new Uri($"pokemon/{name}", UriKind.Relative))
            );

            dynamic speciesResult = JsonConvert.DeserializeObject<JObject>(
                await _httpClient.GetStringAsync((string) searchResult.species.url)
            );

            var flavorTexts = (IEnumerable<dynamic>) speciesResult.flavor_text_entries;
            var enDescription = flavorTexts.First(x => (string) x.language.name == "en");

            var monster = new Pokemon
            {
                Name = (string) searchResult.name,
                Description = (string) enDescription.flavor_text,
                Habitat = (string) speciesResult.habitat.name,
                IsLegendary = (bool) speciesResult.is_legendary
            };

            return monster;
        }
    }
}
