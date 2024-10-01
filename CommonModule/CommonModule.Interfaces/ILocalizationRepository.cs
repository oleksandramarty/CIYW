using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Interfaces;

public interface ILocalizationRepository
{
    Task<Dictionary<string, Dictionary<string, string>>> GetLocalizationDataAllAsync(bool isPublic);
    Task<string> GetLocalizationVersionAsync(bool isPublic);
    Task SetLocalizationVersionAsync(bool isPublic);
}