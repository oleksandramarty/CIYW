using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Currencies;

public class CurrencyResponse: BaseIdEntity<int>, IStatusEntity
{
    public string? Title { get; set; }
    public string? Code { get; set; }
    public string? Symbol { get; set; }
    public string? TitleEn { get; set; }
    public StatusEnum Status { get; set; }
    
    public ICollection<CountryResponse> Countries { get; set; }
}