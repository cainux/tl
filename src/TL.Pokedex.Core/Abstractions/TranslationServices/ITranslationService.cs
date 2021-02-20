using System;
using System.Threading.Tasks;

namespace TL.Pokedex.Core.Abstractions.TranslationServices
{
    public static class Translators
    {
        public static string Yoda { get; } = "yoda";
        public static string Shakespeare { get; } = "shakespeare";
    }

    public class TranslationException : Exception { }

    public interface ITranslationService
    {
        Task<string> GetTranslationAsync(string translator, string source);
    }
}
