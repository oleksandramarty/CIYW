using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Responses.Localizations;

namespace CommonModule.Interfaces;

public interface ILocalizationRepository
{
    Task<LocalizationsResponse> GetLocalizationDataAllAsync(bool isPublic);
    Task<string> GetLocalizationVersionAsync(bool isPublic);
    Task SetLocalizationVersionAsync(bool isPublic);
}