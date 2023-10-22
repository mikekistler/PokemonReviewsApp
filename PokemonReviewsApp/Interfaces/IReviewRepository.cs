using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> ListReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsForAPokemon(int pokemonId);
        bool ReviewExists(int id);
    }
}
