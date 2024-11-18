using AutoMapper;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Localizations;
using Localizations.Domain;
using Localizations.Domain.Models.Locales;
using Localizations.Mediatr.Mediatr.Localizations.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Localizations.Mediatr.Mediatr.Localizations.Handlers;

public class LocalizationsRequestHandler : IRequestHandler<LocalizationsRequest, LocalizationsResponse>
{
    private readonly ILocalizationRepository localizationCacheRepository;
    private readonly IReadGenericRepository<int, LocaleEntity, LocalizationsDataContext> localeRepository;

    public LocalizationsRequestHandler(
        ILocalizationRepository localizationCacheRepository,
        IReadGenericRepository<int, LocaleEntity, LocalizationsDataContext> localeRepository
    )
    {
        this.localizationCacheRepository = localizationCacheRepository;
        this.localeRepository = localeRepository;
    }

    public async Task<LocalizationsResponse> Handle(LocalizationsRequest request,
        CancellationToken cancellationToken)
    {
        string currentVersion = await this.localizationCacheRepository.LocalizationVersionAsync(request.IsPublic);

        if (LocalizationExtension.IsDictionaryActual(request.Version, currentVersion))
        {
            return new LocalizationsResponse
            {
                Data = new List<LocalizationResponse>(),
                Version = currentVersion
            };
        }

        LocalizationsResponse response =
            await this.localizationCacheRepository.LocalizationDataAllAsync(request.IsPublic);

        if (response.Data == null || response.Data.Count == 0)
        {
            var locales = await this.localeRepository.ListAsync(
                null,
                cancellationToken,
                loc => loc.Include(l => l.Localizations));
            
            response.Data = locales.Select(loc => new LocalizationResponse
            {
                Locale = loc.IsoCode,
                Items = loc.Localizations
                    .Where(l => l.IsPublic == request.IsPublic)
                    .Select(l => new LocalizationItemResponse(l?.Key, l?.Value)).ToList()
            }).ToList();
            
            await this.localizationCacheRepository.ReinitializeLocalizationDataAsync(
                response,
                request.IsPublic);
            await this.localizationCacheRepository.SetLocalizationVersionAsync(request.IsPublic);
        }
        
        if (string.IsNullOrEmpty(currentVersion))
        {
            currentVersion = await this.localizationCacheRepository.LocalizationVersionAsync(request.IsPublic);
        }

        response.Version = currentVersion;

        return response;
    }
}