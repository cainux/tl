using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

            var searchResponse = await _httpClient.GetAsync(new Uri($"pokemon/{name}", UriKind.Relative));

            if (!searchResponse.IsSuccessStatusCode)
            {
                _logger.LogDebug("{PokemonName} not found", name);
                return null;
            }

            var searchResult = JsonConvert.DeserializeAnonymousType(
                await searchResponse.Content.ReadAsStringAsync(),
                new
                {
                    name = string.Empty,
                    species = new
                    {
                        url = string.Empty
                    }
                }
            );

            var speciesResponse = await _httpClient.GetAsync(searchResult.species.url);

            if (!speciesResponse.IsSuccessStatusCode)
            {
                _logger.LogDebug("Species fetch ({SpeciesUrl}) for {PokemonName} did not return a success code", searchResult.species.url, name);
                return null;
            }

            var speciesResult = JsonConvert.DeserializeAnonymousType(
                await speciesResponse.Content.ReadAsStringAsync(),
                new
                {
                    habitat = new
                    {
                        name = string.Empty
                    },
                    is_legendary = false,
                    flavor_text_entries = new[]
                    {
                        new
                        {
                            flavor_text = string.Empty,
                            language = new
                            {
                                name = string.Empty
                            }
                        }
                    }
                }
            );

            var enDescription = speciesResult.flavor_text_entries.First(x => x.language.name == "en");

            var monster = new Pokemon
            {
                Name = searchResult.name,
                Description = Regex.Unescape(enDescription.flavor_text),
                Habitat = speciesResult.habitat.name,
                IsLegendary = speciesResult.is_legendary
            };

            return monster;
        }
    }
}
