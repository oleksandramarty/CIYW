using CommonModule.Interfaces;
using CommonModule.Shared.Core;
using CommonModule.Shared.Responses.Dictionaries;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class SiteSettingsRequestHandler: IRequestHandler<SiteSettingsRequest, SiteSettingsResponse>
{
    private readonly ICacheBaseRepository<Guid> _cacheBaseRepository;
    
    public SiteSettingsRequestHandler(ICacheBaseRepository<Guid> cacheBaseRepository)
    {
        _cacheBaseRepository = cacheBaseRepository;
    }
    
    public async Task<SiteSettingsResponse> Handle(SiteSettingsRequest request, CancellationToken cancellationToken)
    {
        SiteSettingsResponse response = new SiteSettingsResponse
        {
            //TODO set locale if authorized
            Locale = "en",
            Version = new CacheVersionResponse
            {
                Category = await _cacheBaseRepository.CacheVersionAsync("category") ?? VersionExtension.GenerateVersion(),
                Country = await _cacheBaseRepository.CacheVersionAsync("country") ?? VersionExtension.GenerateVersion(),
                Currency = await _cacheBaseRepository.CacheVersionAsync("currency") ?? VersionExtension.GenerateVersion(),
                Localization = await _cacheBaseRepository.CacheVersionAsync("localization") ?? VersionExtension.GenerateVersion(),
                LocalizationPublic = await _cacheBaseRepository.CacheVersionAsync("localization_public") ?? VersionExtension.GenerateVersion(),
                Locale = await _cacheBaseRepository.CacheVersionAsync("locale") ?? VersionExtension.GenerateVersion(),
                Frequency = await _cacheBaseRepository.CacheVersionAsync("frequency") ?? VersionExtension.GenerateVersion(),
                BalanceType = await _cacheBaseRepository.CacheVersionAsync("balancetype") ?? VersionExtension.GenerateVersion(),
                IconCategory = await _cacheBaseRepository.CacheVersionAsync("iconcategory") ?? VersionExtension.GenerateVersion(),
            }
        };

        return response;
    }
}