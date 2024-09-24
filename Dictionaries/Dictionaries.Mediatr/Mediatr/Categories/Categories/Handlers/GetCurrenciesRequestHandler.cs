using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Currencies;
using Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Handlers;

public class GetCurrenciesRequestHandler: MediatrDictionariesBase<GetCurrenciesRequest, Currency, CurrencyResponse>
{
    public GetCurrenciesRequestHandler(
        IMapper mapper,
        IReadGenericRepository<int, Currency, DictionariesDataContext> dictionaryRepository) : base(mapper, dictionaryRepository)
    {
    }
}