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

        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category = dataContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            if (owner == null || category == null)
                return false;

            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon
            };
            dataContext.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };
            dataContext.Add(pokemonCategory);

            dataContext.Add(pokemon);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
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

        public bool ReplacePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var owner = dataContext.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
            var category = dataContext.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

            if (owner == null || category == null)
                return false;

            var pokemonOwner = new PokemonOwner()
            {
                Owner = owner,
                Pokemon = pokemon
            };
            dataContext.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon
            };
            dataContext.Add(pokemonCategory);

            dataContext.Update(pokemon);
            return Save();
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            dataContext.Remove(pokemon);
            return Save();
        }
    }
}
