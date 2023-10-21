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

        public ICollection<Pokemon> GetPokemon()
        {
            return dataContext.Pokemon.OrderBy(p => p.Id).ToList();
        }
    }
}
