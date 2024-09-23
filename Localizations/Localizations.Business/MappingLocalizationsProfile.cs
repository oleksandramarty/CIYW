using AutoMapper;
using CommonModule.Shared.Responses.Localizations;
using Localizations.Domain.Models.Locales;

namespace Localizations.Business;

public class MappingLocalizationsProfile: Profile
{
    public MappingLocalizationsProfile()
    {
        this.CreateMap<Locale, LocaleResponse>();
    }
}