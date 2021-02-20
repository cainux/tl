using System.Threading.Tasks;
using TL.Pokedex.Core.Entities;

namespace TL.Pokedex.Core.Abstractions.Repositories
{
    public interface IPocketMonsterRepository
    {
        Task<PocketMonster> GetAsync(string name);
    }
}
