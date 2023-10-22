using PokemonReviewsApp.Data;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Repository
{
    public class PokemonRepository: IPokemonRepository
    {
        private readonly DataContext dataContext;

        public PokemonRepository(DataContext context)
        {
            dataContext = context;
        }

        public Pokemon GetPokemon(int id)
        {
            return dataContext.Pokemon.Where(p => p.Id == id).FirstOrDefault();
        }

        public Pokemon GetPokemon(string name)
        {
            return dataContext.Pokemon.Where(p => p.Name == name).FirstOrDefault();
        }

        public decimal GetPokemonRating(int id)
        {
            var reviews = dataContext.Reviews.Where(r => r.Pokemon.Id == id);
            if (reviews.Count() <= 0)
                return 0;

            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
        }

        public ICollection<Pokemon> ListPokemon()
        {
            return dataContext.Pokemon.OrderBy(p => p.Id).ToList();
        }

        public bool PokemonExists(int id)
        {
            return dataContext.Pokemon.Any(p => p.Id == id);
        }
    }
}
