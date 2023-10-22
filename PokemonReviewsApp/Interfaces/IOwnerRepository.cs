using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> ListOwners();
        Owner GetOwner(int id);
        ICollection<Owner> GetOwnersForAPokemon(int pokemonId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool OwnerExists(int id);
        bool CreateOwner(Owner owner);
    }
}
