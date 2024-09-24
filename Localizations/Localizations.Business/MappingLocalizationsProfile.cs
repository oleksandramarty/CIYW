using AutoMapper;
using CommonModule.Shared.Responses.Localizations;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Localizations.Domain.Models.Locales;

namespace Localizations.Business;

public class MappingLocalizationsProfile: Profile
{
    public MappingLocalizationsProfile()
    {
        this.CreateMap<Locale, LocaleResponse>();
    }
}