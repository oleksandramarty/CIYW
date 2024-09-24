using CommonModule.Shared.Common;
using Dictionaries.Domain.Models.Countries;

namespace Dictionaries.Domain.Models.Currencies;

public class Currency: BaseIdEntity<int>
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string Symbol { get; set; }
    public string TitleEn { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<CountryCurrency> Countries { get; set; }
}