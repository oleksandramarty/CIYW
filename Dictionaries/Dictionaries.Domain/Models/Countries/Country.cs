using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Dictionaries.Domain.Models.Countries;

public class Country: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string TitleEn { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<CountryCurrency> Currencies { get; set; }
}