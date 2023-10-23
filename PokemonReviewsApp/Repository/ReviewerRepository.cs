using PokemonReviewsApp.Data;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext dataContext;

        public ReviewerRepository(DataContext context)
        {
            dataContext = context;
        }

        public ICollection<Reviewer> ListReviewers()
        {
            return dataContext.Reviewers.ToList();
        }

        public Reviewer GetReviewer(int id)
        {
            return dataContext.Reviewers.Where(r => r.Id == id).FirstOrDefault();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return dataContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerExists(int id)
        {
            return dataContext.Reviewers.Any(r => r.Id == id);
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            dataContext.Add(reviewer);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
