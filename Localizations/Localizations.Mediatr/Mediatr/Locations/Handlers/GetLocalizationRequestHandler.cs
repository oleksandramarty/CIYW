using CommonModule.Interfaces;
using CommonModule.Shared.Responses.Localizations;
using Localizations.Mediatr.Mediatr.Locations.Requests;
using MediatR;

namespace Localizations.Mediatr.Mediatr.Locations.Handlers;

public class GetLocalizationRequestHandler: IRequestHandler<GetLocalizationRequest, LocalizationsResponse>
{
    private readonly ILocalizationRepository localizationRepository;
    private readonly IAuthRepository authRepository;

    public GetLocalizationRequestHandler(
        ILocalizationRepository localizationRepository,
        IAuthRepository authRepository)
    {
        this.localizationRepository = localizationRepository;
        this.authRepository = authRepository;
    }

    public async Task<LocalizationsResponse> Handle(GetLocalizationRequest request, CancellationToken cancellationToken)
    {
        bool isAuthenticated = this.authRepository.IsAuthenticated();

        LocalizationsResponse response = new LocalizationsResponse();
        response.Data = await this.localizationRepository.GetLocalizationDataAllAsync(!isAuthenticated);

        return response;
    }
}