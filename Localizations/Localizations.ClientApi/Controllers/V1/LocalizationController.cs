using CommonModule.Core;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Localizations;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Localizations.ClientApi.Controllers.V1;

[ApiController]
[Route("api/v1/localizations")]
public class LocalizationController : BaseController
{
    private readonly IMediator mediator;

    public LocalizationController(IMediator mediator) : base(mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(LocalizationsResponse), 200)]
    public async Task<IActionResult> GetLocalizationsAsync([FromBody]GetLocalizationsRequest request, CancellationToken cancellationToken)
    {
        LocalizationsResponse response = await this.mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("public")]
    [ProducesResponseType(typeof(LocalizationsResponse), 200)]
    public async Task<IActionResult> GetPublicLocalizationsAsync([FromBody]GetLocalizationsRequest request, CancellationToken cancellationToken)
    {
        Guid? userId = await this.GetCurrentUserIdAsync(cancellationToken);

        if (!userId.HasValue)
        {
            request.IsPublic = true;
        }
        
        LocalizationsResponse response = await this.mediator.Send(request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("locales")]
    [ProducesResponseType(typeof(VersionedList<LocaleResponse>), 200)]
    public async Task<IActionResult> GetLocalesAsync([FromBody]GetLocalesRequest request, CancellationToken cancellationToken)
    {
        VersionedList<LocaleResponse> response = await this.mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}