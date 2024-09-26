using CommonModule.Core;
using CommonModule.Facade;
using CommonModule.Shared.Responses.Dictionaries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dictionaries.ClientApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/siteSettings")]
public class SiteSettingController : BaseController
{
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SiteSettingsResponse), 200)]
    public IActionResult GetSettingsAsync()
    {
        SiteSettingsResponse response = new SiteSettingsResponse
        {
            //TODO set locale if authorized
            Locale = "en",
        };
        return Ok(response);
    }
}