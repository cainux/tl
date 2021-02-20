using System.Threading.Tasks;
using TL.Pokedex.Core.Entities;

namespace TL.Pokedex.Core.Abstractions.Repositories
{
    public interface IPokemonRepository
    {
        Task<Pokemon> GetAsync(string name);
    }
}
