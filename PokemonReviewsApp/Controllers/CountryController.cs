using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewsApp.Dto;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private readonly ICountryRepository countryRepository;
        private readonly IMapper mapper;

        public CountryController(ICountryRepository repository, IMapper mapper)
        {
            countryRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        public IActionResult ListCountries()
        {
            var countries = countryRepository.ListCountries();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<CountryDto>>(countries));
        }

        [HttpGet("{id}", Name = "GetCountry")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int id)
        {
            var country = countryRepository.GetCountry(id);

            if (country == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<CountryDto>(country));    
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = countryRepository.GetCountryByOwner(ownerId);

            if (country == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<CountryDto>(country));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public IActionResult CreateCountry([FromBody]CountryDto countryToCreate)
        {
            if (countryToCreate == null)
                return BadRequest(ModelState);

            var country = countryRepository.ListCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", $"Country {countryToCreate.Name} already exists.");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            country = mapper.Map<Country>(countryToCreate);

            if (!countryRepository.CreateCountry(country))
            {
                ModelState.AddModelError("", $"Something went wrong saving the country {country.Name}.");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new { id = country.Id }, mapper.Map<CountryDto>(country));
        }
    }
}
