using CommonModule.Core;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using Dictionaries.Mediatr.Mediatr.Categories.Categories.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dictionaries.ClientApi.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/dictionaries")]
public class DictionaryController: BaseController
{
    private readonly IMediator mediator;
    
    public DictionaryController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpGet("countries")]
    [ProducesResponseType(typeof(List<CountryResponse>), 200)]
    public async Task<IActionResult> GetCountriesAsync()
    {
        List<CountryResponse> response = await mediator.Send(new GetCountriesRequest());
        return Ok(response);
    }
    
    [HttpGet("currencies")]
    [ProducesResponseType(typeof(List<CurrencyResponse>), 200)]
    public async Task<IActionResult> GetCurrenciesAsync()
    {
        List<CurrencyResponse> response = await mediator.Send(new GetCurrenciesRequest());
        return Ok(response);
    }
    
    [HttpGet("categories")]
    [ProducesResponseType(typeof(List<TreeNodeResponse<CategoryResponse>>), 200)]
    public async Task<IActionResult> GetCategoriesAsync()
    {
        List<TreeNodeResponse<CategoryResponse>> response = await mediator.Send(new GetCategoriesRequest());
        return Ok(response);
    }
}