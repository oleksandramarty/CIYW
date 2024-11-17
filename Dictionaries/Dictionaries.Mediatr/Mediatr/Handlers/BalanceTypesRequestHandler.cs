using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Balances;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class BalanceTypesRequestHandler(
    IDictionaryRepository<int, BalanceTypeEntity, BalanceTypeResponse, DictionariesDataContext> dictionaryRepository)
    : MediatrDictionaryBase<BalanceTypesRequest, int, BalanceTypeEntity, BalanceTypeResponse, DictionariesDataContext>(
        dictionaryRepository), IRequestHandler<BalanceTypesRequest, VersionedListResponse<BalanceTypeResponse>>;