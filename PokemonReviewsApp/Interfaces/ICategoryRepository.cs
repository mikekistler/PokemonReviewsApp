using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> ListCategories();
        Category GetCategory(int id);
        ICollection<Pokemon> ListPokemonInCategory(int id);
        bool CategoryExists(int id);
        bool CreateCategory(Category category);
    }
}
