using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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

        public Task<string> GetTranslationAsync(string translator, string source)
        {
            throw new NotImplementedException();
        }
    }
}
