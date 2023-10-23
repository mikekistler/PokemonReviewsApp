using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewsApp.Dto;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository pokemonRepository;
        private readonly IMapper mapper;

        public PokemonController(IPokemonRepository repository, IMapper mapper)
        {
            pokemonRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult ListPokemon()
        {
            var pokemon = pokemonRepository.ListPokemon();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<PokemonDto>>(pokemon));
        }

        [HttpGet("{id}", Name = "GetPokemon")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int id)
        {
            var pokemon = pokemonRepository.GetPokemon(id);

            if (pokemon == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<PokemonDto>(pokemon));
        }

        [HttpGet("{id}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int id)
        {
            if (!pokemonRepository.PokemonExists(id))
                return NotFound();

            var rating = pokemonRepository.GetPokemonRating(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemon = pokemonRepository.ListPokemon().
                FirstOrDefault(p => p.Name.ToLower().Trim() == pokemonCreate.Name.ToLower().Trim());

            if (pokemon != null)
            {
                ModelState.AddModelError("", $"Pokemon {pokemon.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            pokemon = mapper.Map<Pokemon>(pokemonCreate);

            if (!pokemonRepository.CreatePokemon(ownerId, categoryId, pokemon))
            {
                ModelState.AddModelError("", $"Something went wrong saving the pokemon " +
                                       $"{pokemon.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPokemon", new { id = pokemon.Id }, mapper.Map<PokemonDto>(pokemon));
        }
    }
}
