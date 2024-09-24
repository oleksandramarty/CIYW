using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Handlers;

public class GetCategoriesRequestHandler : MediatrDictionariesBase<GetCategoriesRequest, Category, CategoryResponse>
{
    public GetCategoriesRequestHandler(
        IMapper mapper,
        IReadGenericRepository<int, Category, DictionariesDataContext> dictionaryRepository) : base(mapper, dictionaryRepository)
    {
    }
}