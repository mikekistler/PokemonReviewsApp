using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewsApp.Dto;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IReviewerRepository reviewerRepository;
        private readonly IMapper mapper;

        public ReviewerController(IReviewerRepository repository, IMapper mapper)
        {
            reviewerRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult ListReviewers()
        {
            var reviewers = reviewerRepository.ListReviewers();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<ReviewerDto>>(reviewers));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int id)
        {
            var reviewer = reviewerRepository.GetReviewer(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reviewer == null)
                return NotFound();

            return Ok(mapper.Map<ReviewerDto>(reviewer));
        }

        [HttpGet("{id}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int id)
        {
            if (!reviewerRepository.ReviewerExists(id))
                return NotFound();

            var reviews = reviewerRepository.GetReviewsByReviewer(id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<ReviewDto>>(reviews));
        }
    }
}
