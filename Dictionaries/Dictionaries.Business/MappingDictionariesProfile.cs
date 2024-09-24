using AutoMapper;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using Dictionaries.Domain.Models.Categories;

namespace Dictionaries.Business;

public class MappingDictionariesProfile: Profile
{
    public MappingDictionariesProfile()
    {
        this.CreateMap<Category, CategoryResponse>();
    }
}