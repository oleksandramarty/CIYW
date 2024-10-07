using CommonModule.Core;
using CommonModule.Facade;
using CommonModule.Shared.Responses.Dictionaries;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dictionaries.ClientApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/site-settings")]
public class SiteSettingController : BaseController
{
    private readonly IMediator mediator;

    public SiteSettingController(IMediator mediator) : base(mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SiteSettingsResponse), 200)]
    public async Task<IActionResult> GetSettingsAsync(CancellationToken cancellationToken)
    {
        SiteSettingsResponse response = await this.mediator.Send(new GetSiteSettingsRequest(), cancellationToken);
        return Ok(response);
    }
}