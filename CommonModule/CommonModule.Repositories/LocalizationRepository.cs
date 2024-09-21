using CommonModule.Interfaces;

namespace CommonModule.Repositories;

public class LocalizationRepository: ILocalizationRepository
{
    public Task SetLocalizationDataByLocaleAsync(string locale, string key, string value, bool isPublic = false)
    {
        throw new NotImplementedException();
    }

    public Task SetLocalizationDataAllAsync(string locale, Dictionary<string, Tuple<string, bool>> data)
    {
        throw new NotImplementedException();
    }

    public Task SetLocalizationDataAllAsync(Dictionary<string, Dictionary<string, Tuple<string, bool>>> data)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetLocalizationDataByKeyAsync(string locale, string key, bool isPublic = false)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, string>> GetLocalizationDataByLocaleAsync(string locale, bool isPublic = false)
    {
        throw new NotImplementedException();
    }

    public Task<Dictionary<string, Dictionary<string, string>>> GetLocalizationDataAllAsync(bool isPublic = false)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocalizationDataByKeyAsync(string locale, string key)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocalizationDataByLocaleAsync(string locale)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLocalizationDataAllAsync()
    {
        throw new NotImplementedException();
    }
}