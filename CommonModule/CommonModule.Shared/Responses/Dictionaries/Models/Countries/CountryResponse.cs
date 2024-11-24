using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Countries;

public class CountryResponse: BaseIdEntity<int>, IStatusEntity
{
    public string? Title { get; set; }
    public string? Code { get; set; }
    public string? TitleEn { get; set; }
    public StatusEnum Status { get; set; }
    
    public ICollection<CurrencyResponse> Currencies { get; set; }
}