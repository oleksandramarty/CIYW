using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Localizations;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Requests;

public class GetLocalizationsRequest: BaseVersionEntity, IPublicable, IRequest<LocalizationsResponse>
{
    public bool IsPublic { get; set; }
}