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
                var problem = new ProblemDetails()
                {
                    Type = HttpContext.Request.Path + "/ReviewAlreadyExists",
                    Title = "Review already exists",
                    Status = 422,
                    Detail = $"Review by reviewer {reviewerId} for Pokemon {pokemonId} already exists",
                    Instance = HttpContext.TraceIdentifier
                };
                return StatusCode(422, problem);
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

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult ReplaceReview(int id, [FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewToReplace)
        {
            if (reviewToReplace == null)
                return BadRequest(ModelState);

            if (id != reviewToReplace.Id)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!reviewRepository.ReviewExists(id))
                return NotFound();

            var review = mapper.Map<Review>(reviewToReplace);

            if (!reviewRepository.ReplaceReview(reviewerId, pokemonId, review))
            {
                ModelState.AddModelError("", $"Something went wrong replacing the review");
                return StatusCode(500, ModelState);
            }

            return Ok(mapper.Map<ReviewDto>(review));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int id)
        {
            var review = reviewRepository.GetReview(id);

            if (review == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!reviewRepository.DeleteReview(review))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the review");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
