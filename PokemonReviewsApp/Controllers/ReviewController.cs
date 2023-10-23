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

        [HttpGet("{id}", Name = "GetReview")]
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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewToCreate)
        {
            if (reviewToCreate == null)
                return BadRequest(ModelState);

            if (reviewRepository.ReviewExists(reviewerId, pokemonId))
            {
                ModelState.AddModelError("", $"Review by reviewer {reviewerId} for Pokemon {pokemonId} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var review = mapper.Map<Review>(reviewToCreate);

            if (!reviewRepository.CreateReview(reviewerId, pokemonId, review))
            {
                ModelState.AddModelError("", $"Something went wrong saving the review");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReview", new { id = review.Id }, mapper.Map<ReviewDto>(review));
        }
    }
}
