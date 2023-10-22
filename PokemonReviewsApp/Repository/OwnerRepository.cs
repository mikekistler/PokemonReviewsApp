using AutoMapper;
using PokemonReviewsApp.Data;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext dataContext;

        public OwnerRepository(DataContext context)
        {
            dataContext = context;
        }

        public bool CreateOwner(Owner owner)
        {
            dataContext.Add(owner);
            return Save();
        }

        private bool Save()
        {
            var saved = dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Owner GetOwner(int id)
        {
            return dataContext.Owners.Where(o => o.Id == id).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersForAPokemon(int pokemonId)
        {
            return dataContext.PokemonOwners.Where(p => p.Pokemon.Id == pokemonId).Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return dataContext.PokemonOwners.Where(po => po.Owner.Id == ownerId).Select(po => po.Pokemon).ToList();
        }

        public ICollection<Owner> ListOwners()
        {
            return dataContext.Owners.ToList();
        }

        public bool OwnerExists(int id)
        {
            return dataContext.Owners.Any(o => o.Id == id);
        }
    }
}
