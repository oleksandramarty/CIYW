using CommonModule.Shared.Common;

namespace Dictionaries.Domain.Models.Countries;

public class Country: BaseIdEntity<int>
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string TitleEn { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<CountryCurrency> Currencies { get; set; }
}