using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TL.Pokedex.Core.Abstractions.TranslationServices;

namespace TL.Pokedex.Infrastructure.TranslationServices
{
    public class FunTranslationService : ITranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FunTranslationService> _logger;

        public FunTranslationService(HttpClient httpClient, ILogger<FunTranslationService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetTranslationAsync(string translator, string source)
        {
            var uri = new Uri($"{translator}", UriKind.Relative);
            var formBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", source)
            });

            var responseMessage = await _httpClient.PostAsync(uri, formBody);

            if (responseMessage.IsSuccessStatusCode)
            {
                dynamic translationResult = JsonConvert.DeserializeObject<JObject>(await responseMessage.Content.ReadAsStringAsync());

                if ((int) translationResult.success.total > 0)
                {
                    var translation = (string) translationResult.contents.translated;
                    return translation;
                }
            }

            _logger.LogWarning("Failed to get translation, have you hit the rate limit?");

            throw new TranslationException();
        }
    }
}
