using AutoMapper;
using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Currencies;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class GetCurrenciesRequestHandler(
    IDictionaryRepository<int, Currency, CurrencyResponse, DictionariesDataContext> dictionaryRepository)
    : MediatrDictionaryBase<GetCurrenciesRequest, int, Currency, CurrencyResponse, DictionariesDataContext>(
        dictionaryRepository), IRequestHandler<GetCurrenciesRequest, VersionedListResponse<CurrencyResponse>>;