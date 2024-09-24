using CommonModule.Shared.Common;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Currencies;

public class CurrencyResponse: BaseIdEntity<int>
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string Symbol { get; set; }
    public string TitleEn { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<CountryResponse> Countries { get; set; }
}