using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Localizations.Domain.Models.Locales;

public class LocalizationBase: BaseIdEntity<Guid>, IPublicable
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string ValueEn { get; set; }
    
    public int LocaleId { get; set; }
    public Locale Locale { get; set; }
    
    public bool IsPublic { get; set; }
}