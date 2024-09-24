using AutoMapper;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using Expenses.Domain.Models.Categories;

namespace Dictionaries.Business;

public class MappingDictionariesProfile: Profile
{
    public MappingDictionariesProfile()
    {
        this.CreateMap<Category, CategoryResponse>();
    }
}