using System.Threading.Tasks;
using TL.Pokedex.Core.Entities;

namespace TL.Pokedex.Core.Interfaces
{
    public interface IPokemonService
    {
        Task<Pokemon> GetAsync(string name);
        Task<Pokemon> GetTranslatedAsync(string name);
    }
}
