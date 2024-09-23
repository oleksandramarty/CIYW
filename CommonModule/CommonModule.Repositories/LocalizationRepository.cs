using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using Localizations.Domain;
using Localizations.Domain.Models.Locales;
using Microsoft.EntityFrameworkCore;

namespace CommonModule.Repositories
{
    public class LocalizationRepository : ILocalizationRepository
    {
        private readonly LocalizationsDataContext dataContext;

        public LocalizationRepository(LocalizationsDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task SetLocalizationDataByLocaleAsync(string locale, string key, string value, bool isPublic = false)
        {
            var localization = await dataContext.Localizations
                .FirstOrDefaultAsync(l => l.LocaleId == LocalizationExtension.GetLocaleId(locale) && l.Key == key);

            if (localization == null)
            {
                localization = new Localization
                {
                    LocaleId = LocalizationExtension.GetLocaleId(locale),
                    Key = key,
                    Value = value,
                    IsPublic = isPublic
                };
                await dataContext.Localizations.AddAsync(localization);
            }
            else
            {
                localization.Value = value;
                localization.IsPublic = isPublic;
                dataContext.Localizations.Update(localization);
            }

            await dataContext.SaveChangesAsync();
        }

        public async Task SetLocalizationDataAllAsync(string locale, Dictionary<string, Tuple<string, bool>> data)
        {
            foreach (var item in data)
            {
                await SetLocalizationDataByLocaleAsync(locale, item.Key, item.Value.Item1, item.Value.Item2);
            }
        }

        public async Task SetLocalizationDataAllAsync(Dictionary<string, Dictionary<string, Tuple<string, bool>>> data)
        {
            foreach (var localeData in data)
            {
                await SetLocalizationDataAllAsync(localeData.Key, localeData.Value);
            }
        }

        public async Task<string> GetLocalizationDataByKeyAsync(string locale, string key, bool isPublic = false)
        {
            var localization = await dataContext.Localizations
                .FirstOrDefaultAsync(l => l.LocaleId == LocalizationExtension.GetLocaleId(locale) && l.Key == key && l.IsPublic == isPublic);

            return localization?.Value;
        }

        public async Task<Dictionary<string, string>> GetLocalizationDataByLocaleAsync(string locale, bool isPublic = false)
        {
            var localizations = await dataContext.Localizations
                .Where(l => l.LocaleId == LocalizationExtension.GetLocaleId(locale) && l.IsPublic == isPublic)
                .ToListAsync();

            return localizations.ToDictionary(l => l.Key, l => l.Value);
        }

        public async Task<Dictionary<string, Dictionary<string, string>>> GetLocalizationDataAllAsync(bool isPublic = false)
        {
            var localizations = await dataContext.Localizations
                .Include(l => l.Locale)
                .Where(l => !isPublic || l.IsPublic == isPublic)
                .ToListAsync();

            return localizations
                .GroupBy(l => l.Locale.IsoCode)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(l => l.Key, l => l.Value)
                );
        }

        public async Task DeleteLocalizationDataByKeyAsync(string locale, string key)
        {
            var localization = await dataContext.Localizations
                .FirstOrDefaultAsync(l => l.LocaleId == LocalizationExtension.GetLocaleId(locale) && l.Key == key);

            if (localization != null)
            {
                dataContext.Localizations.Remove(localization);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteLocalizationDataByLocaleAsync(string locale)
        {
            var localizations = await dataContext.Localizations
                .Where(l => l.LocaleId == LocalizationExtension.GetLocaleId(locale))
                .ToListAsync();

            if (localizations.Any())
            {
                dataContext.Localizations.RemoveRange(localizations);
                await dataContext.SaveChangesAsync();
            }
        }

        public async Task DeleteLocalizationDataAllAsync()
        {
            var localizations = await dataContext.Localizations.ToListAsync();

            if (localizations.Any())
            {
                dataContext.Localizations.RemoveRange(localizations);
                await dataContext.SaveChangesAsync();
            }
        }
    }
}