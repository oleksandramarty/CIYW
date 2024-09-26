using CommonModule.Facade;
using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Localizations;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Handlers;

public class GetLocalizationsRequestHandler: IRequestHandler<GetLocalizationsRequest, LocalizationsResponse>
{
    private readonly ILocalizationRepository localizationRepository;
    private readonly IAuthRepository authRepository;

    public GetLocalizationsRequestHandler(
        ILocalizationRepository localizationRepository,
        IAuthRepository authRepository)
    {
        this.localizationRepository = localizationRepository;
        this.authRepository = authRepository;
    }

    public async Task<LocalizationsResponse> Handle(GetLocalizationsRequest request, CancellationToken cancellationToken)
    {
        bool isAuthenticated = this.authRepository.IsAuthenticated();

        LocalizationsResponse response = new LocalizationsResponse();
        response.Data = await this.localizationRepository.GetLocalizationDataAllAsync(!isAuthenticated);
        response.Version = await this.localizationRepository.GetLocalizationVersionAsync();
        
        return response;
    }
}