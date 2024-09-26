using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Requests;

public class GetCountriesRequest: BaseVersionEntity, IRequest<VersionedList<CountryResponse>>
{
    
}