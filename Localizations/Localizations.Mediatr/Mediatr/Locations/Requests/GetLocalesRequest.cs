using CommonModule.Shared.Responses.Localizations.Models.Locales;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Requests;

public class GetLocalesRequest: IRequest<List<LocaleResponse>>
{
    
}