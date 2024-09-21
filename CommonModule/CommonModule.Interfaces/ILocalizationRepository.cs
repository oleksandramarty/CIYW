namespace CommonModule.Interfaces;

public interface ILocalizationRepository
{
    Task SetLocalizationDataByLocaleAsync(string locale, string key, string value, bool isPublic = false);
    Task SetLocalizationDataAllAsync(string locale, Dictionary<string, Tuple<string, bool>> data);
    Task SetLocalizationDataAllAsync(Dictionary<string, Dictionary<string, Tuple<string, bool>>> data);
    Task<string> GetLocalizationDataByKeyAsync(string locale, string key, bool isPublic = false);
    Task<Dictionary<string, string>> GetLocalizationDataByLocaleAsync(string locale, bool isPublic = false);
    Task<Dictionary<string, Dictionary<string, string>>> GetLocalizationDataAllAsync(bool isPublic = false);
    Task DeleteLocalizationDataByKeyAsync(string locale, string key);
    Task DeleteLocalizationDataByLocaleAsync(string locale);
    Task DeleteLocalizationDataAllAsync();
}