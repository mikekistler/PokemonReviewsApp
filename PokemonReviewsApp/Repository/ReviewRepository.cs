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
            // This is the Fluent API way of doing this:
            //return dataContext.Reviews.Where(r => r.Id == id).FirstOrDefault();

            // This is the LINQ way of doing this:
            return (Review)(from review in dataContext.Reviews
                   where review.Id == id
                     select review);
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
            //return dataContext.Reviews
            //    .Include(r => r.Reviewer)
            //    .Include(r => r.Pokemon)
            //    .Any(r => r.Reviewer.Id == reviewerId && r.Pokemon.Id == pokemonId);

            // Here's how to do this without using Include():
            //return dataContext.Reviews
            //    .Any(r => EF.Property<int>(r, "ReviewerId") == reviewerId
            //            && EF.Property<int>(r, "PokemonId") == pokemonId);

            // Here's how to do this with a raw SQL query:
            var sql = "SELECT * FROM Reviews WHERE ReviewerId = {0} AND PokemonId = {1}";
            return dataContext.Reviews.FromSqlRaw(sql, reviewerId, pokemonId).Any();
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

        public bool DeleteReview(Review review)
        {
            dataContext.Remove(review);
            return Save();
        }
    }
}
