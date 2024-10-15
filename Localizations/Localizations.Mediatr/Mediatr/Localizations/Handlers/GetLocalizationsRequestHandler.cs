using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Localizations;
using Localizations.Domain;
using Localizations.Domain.Models.Locales;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Localizations.Mediatr.Mediatr.Localizations.Handlers;

public class GetLocalizationsRequestHandler : IRequestHandler<GetLocalizationsRequest, LocalizationsResponse>
{
    private readonly ILocalizationRepository localizationCacheRepository;
    private readonly IReadGenericRepository<Guid, Locale, LocalizationsDataContext> localeRepository;

    public GetLocalizationsRequestHandler(
        ILocalizationRepository localizationCacheRepository,
        IReadGenericRepository<Guid, Locale, LocalizationsDataContext> localeRepository
    )
    {
        this.localizationCacheRepository = localizationCacheRepository;
        this.localeRepository = localeRepository;
    }

    public async Task<LocalizationsResponse> Handle(GetLocalizationsRequest request,
        CancellationToken cancellationToken)
    {
        string? currentVersion = await this.localizationCacheRepository.GetLocalizationVersionAsync(request.IsPublic);

        if (LocalizationExtension.IsDictionaryActual(request.Version, currentVersion))
        {
            return new LocalizationsResponse
            {
                Data = new List<LocalizationResponse>(),
                Version = currentVersion
            };
        }

        LocalizationsResponse response =
            await this.localizationCacheRepository.GetLocalizationDataAllAsync(request.IsPublic);

        if (response.Data == null || response.Data.Count == 0)
        {
            var locales = await this.localeRepository.GetListAsync(
                null,
                cancellationToken,
                loc => loc.Include(l => l.Localizations));
            
            response.Data = locales.Select(loc => new LocalizationResponse
            {
                Locale = loc.IsoCode,
                Items = loc.Localizations
                    .Where(l => l.IsPublic == request.IsPublic)
                    .Select(l => new LocalizationItemResponse
                {
                    Key = l.Key,
                    Value = l.Value
                }).ToList()
            }).ToList();
            
            await this.localizationCacheRepository.ReinitializeLocalizationDataAsync(
                response,
                request.IsPublic);
            await this.localizationCacheRepository.SetLocalizationVersionAsync(request.IsPublic);
        }
        
        if (string.IsNullOrEmpty(currentVersion))
        {
            currentVersion = await this.localizationCacheRepository.GetLocalizationVersionAsync(request.IsPublic);
        }

        response.Version = currentVersion;

        return response;
    }
}