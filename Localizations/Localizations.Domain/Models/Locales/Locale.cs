using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;

namespace Localizations.Domain.Models.Locales;

public class Locale: BaseIdEntity<int>, IActivatable
{
    public string IsoCode { get; set; }
    public string Title { get; set; }
    public string TitleEn { get; set; }
    public string TitleNormalized { get; set; }
    public string TitleEnNormalized { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public LocaleEnum LocaleEnum { get; set; }
    public string Culture { get; set; }
    
    public ICollection<Localization> Localizations { get; set; }
}