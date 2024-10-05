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
                Category = await this.cacheBaseRepository.GetCacheVersionAsync("category"),
                Country = await this.cacheBaseRepository.GetCacheVersionAsync("country"),
                Currency = await this.cacheBaseRepository.GetCacheVersionAsync("currency"),
                Localization = await this.cacheBaseRepository.GetCacheVersionAsync("localization"),
                LocalizationPublic = await this.cacheBaseRepository.GetCacheVersionAsync("localization_public"),
                Locale = await this.cacheBaseRepository.GetCacheVersionAsync("locale"),
                Frequency = await this.cacheBaseRepository.GetCacheVersionAsync("frequency")
            }
        };

        return response;
    }
}