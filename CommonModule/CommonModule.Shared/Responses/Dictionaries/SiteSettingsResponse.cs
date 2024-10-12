namespace CommonModule.Shared.Responses.Dictionaries;

public class SiteSettingsResponse
{
    public string Locale { get; set; }
    public CacheVersionResponse Version { get; set; }
}

public class CacheVersionResponse
{
    public string LocalizationPublic { get; set; }
    public string Localization { get; set; }
    public string Category { get; set; }
    public string Currency { get; set; }
    public string Country { get; set; }
    public string Locale { get; set; }
    public string Frequency { get; set; }
    public string BalanceType { get; set; }
}
