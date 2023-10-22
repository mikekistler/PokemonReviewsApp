using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewsApp.Dto;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IReviewRepository reviewRepository;
        private readonly IMapper mapper;

        public ReviewController(IReviewRepository repository, IMapper mapper)
        {
            reviewRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult ListReviews()
        {
            var reviews = reviewRepository.ListReviews();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<ReviewDto>>(reviews));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult GetReview(int id)
        {
            var review = reviewRepository.GetReview(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (review == null)
                return NotFound();

            return Ok(mapper.Map<ReviewDto>(review));
        }

        [HttpGet("pokemon/{pokemonId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsForAPokemon(int pokemonId)
        {
            var reviews = reviewRepository.GetReviewsForAPokemon(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<ReviewDto>>(reviews));
        }
    }
}
