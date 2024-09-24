using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;

public class GetCountriesRequest: IRequest<List<CountryResponse>>
{
    
}