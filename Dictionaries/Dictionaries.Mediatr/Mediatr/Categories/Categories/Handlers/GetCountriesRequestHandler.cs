using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Countries;
using Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Handlers;

public class GetCountriesRequestHandler: MediatrDictionariesBase<GetCountriesRequest, Country, CountryResponse>
{
    public GetCountriesRequestHandler(
        IMapper mapper,
        IReadGenericRepository<int, Country, DictionariesDataContext> dictionaryRepository) : base(mapper, dictionaryRepository)
    {
    }
}