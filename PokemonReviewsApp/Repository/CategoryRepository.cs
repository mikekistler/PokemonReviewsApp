using PokemonReviewsApp.Data;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext dataContext;

        public CategoryRepository(DataContext context)
        {
            dataContext = context;
        }

        public bool CategoryExists(int id)
        {
            return dataContext.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            dataContext.Add(category);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Category GetCategory(int id)
        {
            return dataContext.Categories.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Category> ListCategories()
        {
            return dataContext.Categories.ToList();
        }

        public ICollection<Pokemon> ListPokemonInCategory(int id)
        {
            return dataContext.PokemonCategories.Where(c => c.CategoryId == id).Select(c => c.Pokemon).ToList();
        }
    }
}
