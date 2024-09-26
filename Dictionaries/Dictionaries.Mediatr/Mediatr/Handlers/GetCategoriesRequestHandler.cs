using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class GetCategoriesRequestHandler(
    ITreeDictionaryRepository<int, int?, Category, CategoryResponse, DictionariesDataContext> treeDictionaryRepository)
    : MediatrTreeDictionaryBase<GetCategoriesRequest, int, int?, Category, CategoryResponse, DictionariesDataContext>(
        treeDictionaryRepository);