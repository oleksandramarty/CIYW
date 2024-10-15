using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Dictionaries;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Mediatr.Mediatr.Requests;
using MediatR;

namespace Dictionaries.Mediatr.Mediatr.Handlers;

public class GetSiteSettingsRequestHandler: IRequestHandler<GetSiteSettingsRequest, SiteSettingsResponse>
{
    private readonly ICacheBaseRepository<Guid> cacheBaseRepository;
    
    public GetSiteSettingsRequestHandler(ICacheBaseRepository<Guid> cacheBaseRepository)
    {
        this.cacheBaseRepository = cacheBaseRepository;
    }
    
    public async Task<SiteSettingsResponse> Handle(GetSiteSettingsRequest request, CancellationToken cancellationToken)
    {
        SiteSettingsResponse response = new SiteSettingsResponse
        {
            //TODO set locale if authorized
            Locale = "en",
            Version = new CacheVersionResponse
            {
                Category = await this.cacheBaseRepository.GetCacheVersionAsync("category") ?? Guid.NewGuid().ToString("N").ToUpper(),
                Country = await this.cacheBaseRepository.GetCacheVersionAsync("country") ?? Guid.NewGuid().ToString("N").ToUpper(),
                Currency = await this.cacheBaseRepository.GetCacheVersionAsync("currency") ?? Guid.NewGuid().ToString("N").ToUpper(),
                Localization = await this.cacheBaseRepository.GetCacheVersionAsync("localization") ?? Guid.NewGuid().ToString("N").ToUpper(),
                LocalizationPublic = await this.cacheBaseRepository.GetCacheVersionAsync("localization_public") ?? Guid.NewGuid().ToString("N").ToUpper(),
                Locale = await this.cacheBaseRepository.GetCacheVersionAsync("locale") ?? Guid.NewGuid().ToString("N").ToUpper(),
                Frequency = await this.cacheBaseRepository.GetCacheVersionAsync("frequency") ?? Guid.NewGuid().ToString("N").ToUpper(),
                BalanceType = await this.cacheBaseRepository.GetCacheVersionAsync("balancetype") ?? Guid.NewGuid().ToString("N").ToUpper(),
                IconCategory = await this.cacheBaseRepository.GetCacheVersionAsync("iconcategory") ?? Guid.NewGuid().ToString("N").ToUpper(),
            }
        };

        return response;
    }
}