using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> ListReviewers();
        Reviewer GetReviewer(int id);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool ReviewerExists(int id);
        bool CreateReviewer(Reviewer reviewer);
    }
}
