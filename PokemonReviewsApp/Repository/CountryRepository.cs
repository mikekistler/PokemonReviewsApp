using AutoMapper;
using PokemonReviewsApp.Data;
using PokemonReviewsApp.Interfaces;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public CountryRepository(DataContext context, IMapper mapper)
        {
            dataContext = context;
            this.mapper = mapper;
        }
        public bool CountryExists(int id)
        {
            return dataContext.Countries.Any(c => c.Id == id);
        }

        public Country GetCountry(int id)
        {
            return dataContext.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return dataContext.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }

        public ICollection<Owner> GetOwnersFromCountry(int countryId)
        {
            return dataContext.Owners.Where(c => c.Country.Id == countryId).ToList();
        }

        public ICollection<Country> ListCountries()
        {
            return dataContext.Countries.ToList();
        }
    }
}
