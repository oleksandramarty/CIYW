using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Mediatr.Mediatr.Requests;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class CategoriesRequestHandler(
    ITreeDictionaryRepository<int, int?, CategoryEntity, CategoryResponse, DictionariesDataContext> treeDictionaryRepository)
    : MediatrTreeDictionaryBase<CategoriesRequest, int, int?, CategoryEntity, CategoryResponse, DictionariesDataContext>(
        treeDictionaryRepository);