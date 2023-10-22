using AutoMapper;
using PokemonReviewsApp.Dto;
using PokemonReviewsApp.Models;

namespace PokemonReviewsApp.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
        }
    }
}
