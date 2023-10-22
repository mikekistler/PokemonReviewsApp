using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewsApp.Dto;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoryController(ICategoryRepository repository, IMapper mapper)
        {
            categoryRepository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult ListCategories()
        {
            var categories = categoryRepository.ListCategories();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<CategoryDto>>(categories));
        }

        [HttpGet("{id}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int id)
        {
            var category = categoryRepository.GetCategory(id);

            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<CategoryDto>(category));
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        public IActionResult ListPokemonInCategory(int categoryId)
        {
            if (!categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var pokemon = categoryRepository.ListPokemonInCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(mapper.Map<ICollection<PokemonDto>>(pokemon));
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            var category = categoryRepository.ListCategories()
                .Where(c => c.Name.Trim().ToLower() == categoryCreate.Name.Trim().ToLower())
                .FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", $"Category {categoryCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            category = mapper.Map<Category>(categoryCreate);

            if (!categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Something went wrong saving the category {category.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { id = category.Id }, mapper.Map<CategoryDto>(category));
        }   
    }
}
