using CommonModule.Core;
using CommonModule.Shared.Responses.Localizations;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Localizations.ClientApi.Controllers.V1;

[ApiController]
[Route("api/v1/localizations")]
public class LocalizationController : BaseController
{
    private readonly IMediator mediator;

    public LocalizationController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(LocalizationsResponse), 200)]
    public async Task<IActionResult> GetLocalizationsAsync(CancellationToken cancellationToken)
    {
        LocalizationsResponse response = await this.mediator.Send(new GetLocalizationsRequest(), cancellationToken);
        return Ok(response);
    }

    [HttpGet("locales")]
    [ProducesResponseType(typeof(List<LocaleResponse>), 200)]
    public async Task<IActionResult> GetLocalesAsync(CancellationToken cancellationToken)
    {
        List<LocaleResponse> response = await this.mediator.Send(new GetLocalesRequest(), cancellationToken);
        return Ok(response);
    }
}