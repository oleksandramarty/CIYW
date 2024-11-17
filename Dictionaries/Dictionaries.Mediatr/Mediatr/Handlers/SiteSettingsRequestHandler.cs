using CommonModule.Interfaces;
using CommonModule.Shared.Core;
using CommonModule.Shared.Responses.Dictionaries;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class SiteSettingsRequestHandler: IRequestHandler<SiteSettingsRequest, SiteSettingsResponse>
{
    private readonly ICacheBaseRepository<Guid> cacheBaseRepository;
    
    public SiteSettingsRequestHandler(ICacheBaseRepository<Guid> cacheBaseRepository)
    {
        this.cacheBaseRepository = cacheBaseRepository;
    }
    
    public async Task<SiteSettingsResponse> Handle(SiteSettingsRequest request, CancellationToken cancellationToken)
    {
        SiteSettingsResponse response = new SiteSettingsResponse
        {
            //TODO set locale if authorized
            Locale = "en",
            Version = new CacheVersionResponse
            {
                Category = await this.cacheBaseRepository.CacheVersionAsync("category") ?? VersionExtension.GenerateVersion(),
                Country = await this.cacheBaseRepository.CacheVersionAsync("country") ?? VersionExtension.GenerateVersion(),
                Currency = await this.cacheBaseRepository.CacheVersionAsync("currency") ?? VersionExtension.GenerateVersion(),
                Localization = await this.cacheBaseRepository.CacheVersionAsync("localization") ?? VersionExtension.GenerateVersion(),
                LocalizationPublic = await this.cacheBaseRepository.CacheVersionAsync("localization_public") ?? VersionExtension.GenerateVersion(),
                Locale = await this.cacheBaseRepository.CacheVersionAsync("locale") ?? VersionExtension.GenerateVersion(),
                Frequency = await this.cacheBaseRepository.CacheVersionAsync("frequency") ?? VersionExtension.GenerateVersion(),
                BalanceType = await this.cacheBaseRepository.CacheVersionAsync("balancetype") ?? VersionExtension.GenerateVersion(),
                IconCategory = await this.cacheBaseRepository.CacheVersionAsync("iconcategory") ?? VersionExtension.GenerateVersion(),
            }
        };

        return response;
    }
}