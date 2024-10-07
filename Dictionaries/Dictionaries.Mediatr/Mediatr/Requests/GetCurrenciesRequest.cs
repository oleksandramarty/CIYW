using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class GetCurrenciesRequest: BaseVersionEntity, IRequest<VersionedList<CurrencyResponse>>
{
    
}