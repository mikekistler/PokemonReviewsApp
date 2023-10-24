using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> ListReviews();
        Review GetReview(int id);
        ICollection<Review> GetReviewsForAPokemon(int pokemonId);
        bool ReviewExists(int id);
        bool ReviewExists(int reviewerId, int pokemonId);
        bool CreateReview(int reviewerId, int pokemonId, Review review);
        bool ReplaceReview(int reviewerId, int pokemonId, Review review);
    }
}
