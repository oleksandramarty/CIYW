using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Localizations;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Requests;

public class GetLocalizationsRequest: BaseVersionEntity, IRequest<LocalizationsResponse>
{
    public bool IsPublic { get; set; }
}