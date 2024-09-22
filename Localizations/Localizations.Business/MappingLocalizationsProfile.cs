using AutoMapper;
using CommonModule.Shared.Responses.Localizations;
using JobPathfinder.Data.Domain.Models.Locales;

namespace Localizations.Business;

public class MappingLocalizationsProfile: Profile
{
    public MappingLocalizationsProfile()
    {
        this.CreateMap<Locale, LocaleResponse>();
    }
}