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

        [HttpGet("{id}", Name = "GetReviewer")]
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

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var reviewer = reviewerRepository.ListReviewers()
                .FirstOrDefault(r => r.FirstName.Equals(reviewerCreate.FirstName) && r.LastName.Equals(reviewerCreate.LastName));

            if (reviewer != null)
            {
                ModelState.AddModelError("", $"Reviewer {reviewerCreate.FirstName} {reviewerCreate.LastName} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            reviewer = mapper.Map<Reviewer>(reviewerCreate);

            if (!reviewerRepository.CreateReviewer(reviewer))
            {
                ModelState.AddModelError("", $"Something went wrong saving the reviewer " +
                                       $"{reviewer.FirstName} {reviewer.LastName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReviewer", new { id = reviewer.Id }, mapper.Map<ReviewerDto>(reviewer));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult ReplaceReviewer(int id, [FromBody] ReviewerDto reviewerUpdate)
        {
            if (reviewerUpdate == null)
                return BadRequest(ModelState);

            if (id != reviewerUpdate.Id)
                return BadRequest(ModelState);

            if (!reviewerRepository.ReviewerExists(id))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewer = mapper.Map<Reviewer>(reviewerUpdate);

            if (!reviewerRepository.ReplaceReviewer(reviewer))
            {
                ModelState.AddModelError("", $"Something went wrong updating the reviewer " +
                                                          $"{reviewer.FirstName} {reviewer.LastName}");
                return StatusCode(500, ModelState);
            }

            return Ok(mapper.Map<ReviewerDto>(reviewer));
        }
    }
}
