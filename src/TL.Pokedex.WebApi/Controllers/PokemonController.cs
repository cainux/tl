using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TL.Pokedex.Core.Interfaces;

namespace TL.Pokedex.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class PokemonController : ControllerBase
    {
        private readonly IPocketMonsterService _pocketMonsterService;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(IPocketMonsterService pocketMonsterService, ILogger<PokemonController> logger)
        {
            _pocketMonsterService = pocketMonsterService;
            _logger = logger;
        }

        [HttpGet]
        [Route("translated/{name}")]
        public async Task<IActionResult> GetTranslated([FromRoute] string name)
        {
            _logger.LogDebug("Getting Translated {PokemonName}", name);

            var monster = await _pocketMonsterService.GetTranslatedAsync(name);

            if (monster == null)
            {
                return NotFound();
            }

            return Ok(monster);
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get([FromRoute] string name)
        {
            _logger.LogDebug("Getting {PokemonName}", name);

            var monster = await _pocketMonsterService.GetAsync(name);

            if (monster == null)
            {
                return NotFound();
            }

            return Ok(monster);
        }
    }
}
