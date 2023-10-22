using PokemonReviewsApp.Data;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext dataContext;

        public ReviewRepository(DataContext context)
        {
            dataContext = context;
        }

        public Review GetReview(int id)
        {
            return dataContext.Reviews.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsForAPokemon(int pokemonId)
        {
            return dataContext.Reviews.Where(r => r.Pokemon.Id == pokemonId).ToList();
        }

        public ICollection<Review> ListReviews()
        {
            return dataContext.Reviews.ToList();
        }

        public bool ReviewExists(int id)
        {
            return dataContext.Reviews.Any(r => r.Id == id);
        }
    }
}
