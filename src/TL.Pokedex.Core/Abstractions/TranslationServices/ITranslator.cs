using System.Threading.Tasks;

namespace TL.Pokedex.Core.Abstractions.TranslationServices
{
    public interface ITranslator
    {
        Task<string> GetTranslationAsync(string source);
    }
}
