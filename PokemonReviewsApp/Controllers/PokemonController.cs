using Microsoft.AspNetCore.Mvc;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository pokemonRepository;

        public PokemonController(IPokemonRepository repository)
        {
            pokemonRepository = repository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemon()
        {
            var pokemon = pokemonRepository.GetPokemon();

            if (!ModelState.IsValid)
                return new BadRequestObjectResult(ModelState);

            return new OkObjectResult(pokemon);
        }
    }
}
