using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class CurrenciesRequest: BaseVersionEntity, IRequest<VersionedListResponse<CurrencyResponse>>
{
    
}