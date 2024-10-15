using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Requests;

public class GetLocalesRequest: BaseVersionEntity, IRequest<VersionedListResponse<LocaleResponse>>
{
    
}