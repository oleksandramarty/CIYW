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
        bool isAuthenticated = this.authRepository.IsAuthenticated();

        BaseVersionEntity baseVersionEntity = await this.localizationRepository.GetLocalizationVersionAsync();

        LocalizationsResponse response = new LocalizationsResponse();
        
        var currentCount = isAuthenticated ? 
            baseVersionEntity.Count?.Split(":").Select(int.Parse).Max().ToString() :
            baseVersionEntity.Count?.Split(":").Select(int.Parse).Min().ToString();
        
        response.Version = baseVersionEntity.Version;
        response.Count = currentCount;
        
        if (
            !string.IsNullOrEmpty(request.Version) && 
            request.Version.Equals(baseVersionEntity.Version) && 
            !string.IsNullOrEmpty(request.Count) &&
            request.Count.Equals(currentCount))
        {
            response.Data = new Dictionary<string, Dictionary<string, string>>();
        }
        else
        {
            response.Data = await this.localizationRepository.GetLocalizationDataAllAsync(!isAuthenticated);
        }
        
        return response;
    }
}