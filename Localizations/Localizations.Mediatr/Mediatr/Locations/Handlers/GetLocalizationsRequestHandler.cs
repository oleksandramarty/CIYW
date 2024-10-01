using CommonModule.Facade;
using CommonModule.Interfaces;
using CommonModule.Shared.Common.BaseInterfaces;
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
        string currentVersion = await this.localizationRepository.GetLocalizationVersionAsync(request.IsPublic);

        LocalizationsResponse response = new LocalizationsResponse();
        
        response.Version = currentVersion;
        
        if (
            !string.IsNullOrEmpty(request.Version) && 
            request.Version.Equals(currentVersion))
        {
            response.Data = new Dictionary<string, Dictionary<string, string>>();
        }
        else
        {
            response.Data = await this.localizationRepository.GetLocalizationDataAllAsync(request.IsPublic);
        }
        
        return response;
    }
}