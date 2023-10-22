using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewsApp.Dto;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IOwnerRepository ownerRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public OwnerController(IOwnerRepository repository, 
            ICountryRepository countryRepository,
            IMapper mapper)
        {
            ownerRepository = repository;
            this.countryRepository = countryRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult ListOwners()
        {
            var owners = ownerRepository.ListOwners();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<OwnerDto>>(owners));
        }

        [HttpGet("{id}", Name = "GetOwner")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int id)
        {
            var owner = ownerRepository.GetOwner(id);

            if (owner == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<OwnerDto>(owner));
        }

        [HttpGet("{id}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner(int id)
        {
            if (!ownerRepository.OwnerExists(id))
                return NotFound();

            var pokemon = ownerRepository.GetPokemonByOwner(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<PokemonDto>>(pokemon));
        }

        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public IActionResult GetOwnersForAPokemon(int pokemonId)
        {
            var owners = ownerRepository.GetOwnersForAPokemon(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<OwnerDto>>(owners));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerToCreate)
        {
            if (ownerToCreate == null)
                return BadRequest(ModelState);

            var owner = ownerRepository.ListOwners()
                .Where(o => o.Name.Trim().ToUpper() == ownerToCreate.Name.Trim().ToLower())
                .FirstOrDefault();

            if (owner != null)
            {
                ModelState.AddModelError("", $"Owner {ownerToCreate.Name} already exists!");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            owner = mapper.Map<Owner>(ownerToCreate);

            owner.Country = countryRepository.GetCountry(countryId);

            if (!ownerRepository.CreateOwner(owner))
            {
                ModelState.AddModelError("", $"Something went wrong saving {owner.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetOwner", new { id = ownerToCreate.Id }, ownerToCreate);
        }
    }
}
