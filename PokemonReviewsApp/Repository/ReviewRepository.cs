using Microsoft.EntityFrameworkCore;
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

        public bool CreateReview(int reviewerId, int pokemonId, Review review)
        {
            var reviewer = dataContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
            var pokemon = dataContext.Pokemon.Where(p => p.Id == pokemonId).FirstOrDefault();

            if (reviewer == null || pokemon == null)
                return false;

            review.Reviewer = reviewer;
            review.Pokemon = pokemon;

            dataContext.Add(review);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
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

        public bool ReviewExists(int reviewerId, int pokemonId)
        {
            return dataContext.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Pokemon)
                .Any(r => r.Reviewer.Id == reviewerId && r.Pokemon.Id == pokemonId);

        }

        public bool ReplaceReview(int reviewerId, int pokemonId, Review review)
        {
            var reviewer = dataContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
            var pokemon = dataContext.Pokemon.Where(p => p.Id == pokemonId).FirstOrDefault();

            if (reviewer == null || pokemon == null)
                return false;

            review.Reviewer = reviewer;
            review.Pokemon = pokemon;

            dataContext.Update(review);
            return Save();
        }
    }
}
