namespace CommonModule.Core.Extensions;

public static class LocalizationExtension
{
    public static int GetLocaleId(string locale)
    {
        return locale switch
        {
            "en" => 1,
            "es" => 2,
            "fr" => 3,
            "ua" => 4,
            "ru" => 5,
            "de" => 6,
            "it" => 7
        };
    }
    
    public static bool IsDictionaryActual(string? version, string? currentVersion)
    {
        return !string.IsNullOrEmpty(currentVersion) &&
               !string.IsNullOrEmpty(version) &&
               version.Equals(currentVersion);
    }
}