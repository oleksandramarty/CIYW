using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;

namespace CommonModule.Shared.Responses.Localizations;

public class LocaleResponse: BaseIdEntity<int>
{
    public string IsoCode { get; set; }
    public string Title { get; set; }
    public string TitleEn { get; set; }
    public string TitleNormalized { get; set; }
    public string TitleEnormalized { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }
    public LocaleEnum LocaleEnum { get; set; }
    public string Culture { get; set; }
}