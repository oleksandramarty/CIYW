using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Countries;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class GetCountriesRequestHandler(
    IDictionaryRepository<int, CountryEntity, CountryResponse, DictionariesDataContext> dictionaryRepository)
    : MediatrDictionaryBase<GetCountriesRequest, int, CountryEntity, CountryResponse, DictionariesDataContext>(
        dictionaryRepository), IRequestHandler<GetCountriesRequest, VersionedListResponse<CountryResponse>>;