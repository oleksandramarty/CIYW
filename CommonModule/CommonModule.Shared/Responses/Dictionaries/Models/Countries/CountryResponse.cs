using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Countries;

public class CountryResponse: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string TitleEn { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<CurrencyResponse> Currencies { get; set; }
}