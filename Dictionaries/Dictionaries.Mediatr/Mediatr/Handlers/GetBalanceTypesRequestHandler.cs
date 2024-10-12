using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Balances;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class GetBalanceTypesRequestHandler(
    IDictionaryRepository<int, BalanceType, BalanceTypeResponse, DictionariesDataContext> dictionaryRepository)
    : MediatrDictionaryBase<GetBalanceTypesRequest, int, BalanceType, BalanceTypeResponse, DictionariesDataContext>(
        dictionaryRepository), IRequestHandler<GetBalanceTypesRequest, VersionedListResponse<BalanceTypeResponse>>;