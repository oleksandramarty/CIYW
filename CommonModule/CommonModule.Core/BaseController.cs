using CommonModule.Core.Exceptions.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommonModule.Core;

[Route("api/{version}/[controller]")]
[ApiController]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status400BadRequest)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status401Unauthorized)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status403Forbidden)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status404NotFound)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status409Conflict)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status417ExpectationFailed)]
[ProducesResponseType(typeof(ErrorMessageModel), StatusCodes.Status500InternalServerError)]
public class BaseController : Controller
{
    public BaseController()
    {
    }
}