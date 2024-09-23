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
}