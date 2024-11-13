using CommonModule.Core.Mediatr;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Localizations.Domain;
using Localizations.Domain.Models.Locales;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Localizations.Handlers;

public class GetLocalesRequestHandler : IRequestHandler<GetLocalesRequest, VersionedListResponse<LocaleResponse>>
{
    private readonly IDictionaryRepository<int, LocaleEntity, LocaleResponse, LocalizationsDataContext> dictionaryRepository;
    
    public GetLocalesRequestHandler(
        IDictionaryRepository<int, LocaleEntity, LocaleResponse, LocalizationsDataContext> dictionaryRepository
        )
    {
        this.dictionaryRepository = dictionaryRepository;
    }
    
    public async Task<VersionedListResponse<LocaleResponse>> Handle(GetLocalesRequest request, CancellationToken cancellationToken)
    {
        VersionedListResponse<LocaleResponse> response = await this.dictionaryRepository.GetDictionaryAsync(request.Version, cancellationToken);

        response.Items = response.Items.OrderBy(i => i.Id).ToList();
        
        return response;
    }
}