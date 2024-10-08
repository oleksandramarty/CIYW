using CommonModule.Core;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using Dictionaries.Mediatr.Mediatr.Requests;
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
    
    public DictionaryController(IMediator mediator) : base(mediator)
    {
        this.mediator = mediator;
    }
    
    [HttpPost("countries")]
    [ProducesResponseType(typeof(VersionedListResponse<CountryResponse>), 200)]
    public async Task<IActionResult> GetCountriesAsync([FromBody]GetCountriesRequest request, CancellationToken cancellationToken)
    {
        VersionedListResponse<CountryResponse> response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("currencies")]
    [ProducesResponseType(typeof(VersionedListResponse<CurrencyResponse>), 200)]
    public async Task<IActionResult> GetCurrenciesAsync([FromBody]GetCurrenciesRequest request, CancellationToken cancellationToken)
    {
        VersionedListResponse<CurrencyResponse> response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("categories")]
    [ProducesResponseType(typeof(VersionedListResponse<TreeNodeResponse<CategoryResponse>>), 200)]
    public async Task<IActionResult> GetCategoriesAsync([FromBody]GetCategoriesRequest request, CancellationToken cancellationToken)
    {
        VersionedListResponse<TreeNodeResponse<CategoryResponse>> response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("frequencies")]
    [ProducesResponseType(typeof(VersionedListResponse<FrequencyResponse>), 200)]
    public async Task<IActionResult> GetFrequenciesAsync([FromBody]GetFrequenciesRequest request, CancellationToken cancellationToken)
    {
        VersionedListResponse<FrequencyResponse> response = await mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}