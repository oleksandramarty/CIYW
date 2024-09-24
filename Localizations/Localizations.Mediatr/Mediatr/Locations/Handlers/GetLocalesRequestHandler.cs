using AutoMapper;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Localizations;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Localizations.Domain;
using Localizations.Domain.Models.Locales;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Handlers;

public class GetLocalesRequestHandler: IRequestHandler<GetLocalesRequest, List<LocaleResponse>>
{
    // TODO store in redis
    private readonly IMapper mapper;
    private readonly IReadGenericRepository<int, Locale, LocalizationsDataContext> localeRepository;
    
    public GetLocalesRequestHandler(
        IMapper mapper,
        IReadGenericRepository<int, Locale, LocalizationsDataContext> localeRepository)
    {
        this.mapper = mapper;
        this.localeRepository = localeRepository;
    }
    
    public async Task<List<LocaleResponse>> Handle(GetLocalesRequest request, CancellationToken cancellationToken)
    {
        List<Locale> locales = await this.localeRepository.GetListAsync(r => true, cancellationToken);
        List<LocaleResponse> response = 
            locales.Select(l => this.mapper.Map<LocaleResponse>(l)).ToList();
        return response;
    }
}