using System.Threading.Tasks;
using TL.Pokedex.Core.Entities;

namespace TL.Pokedex.Core.Interfaces
{
    public interface IPocketMonsterService
    {
        Task<PocketMonster> GetAsync(string name);
        Task<PocketMonster> GetTranslatedAsync(string name);
    }
}
