using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Interfaces
{
    public interface IPokemonRepository
    {
        ICollection<Pokemon> ListPokemon();
        Pokemon GetPokemon(int id);
        Pokemon GetPokemon(string name);
        decimal GetPokemonRating(int id);
        bool PokemonExists(int id);
        bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    }
}
