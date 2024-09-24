using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;

public class GetCurrenciesRequest: IRequest<List<CurrencyResponse>>
{
    
}