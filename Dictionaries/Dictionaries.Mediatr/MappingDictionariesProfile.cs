using AutoMapper;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using CommonModule.Shared.Responses.Dictionaries.Models.Icons;
using Dictionaries.Domain.Models.Balances;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Domain.Models.Countries;
using Dictionaries.Domain.Models.Currencies;
using Dictionaries.Domain.Models.Expenses;
using Dictionaries.Domain.Models.Icons;

namespace Dictionaries.Mediatr;

public class MappingDictionariesProfile : Profile
{
    public MappingDictionariesProfile()
    {
        CreateCategoryMappings();
        CreateCountryMappings();
        CreateCurrencyMappings();

        CreateMap<Frequency, FrequencyResponse>();
        CreateMap<BalanceType, BalanceTypeResponse>();
        CreateMap<Icon, IconResponse>();
        CreateMap<IconCategory, IconCategoryResponse>();
    }

    private void CreateCategoryMappings()
    {
        CreateMap<Category, CategoryResponse>()
            .ForMember(dest => dest.Children, opt => opt.Ignore());
    }

    private void CreateCountryMappings()
    {
        CreateMap<Country, CountryResponse>()
            .ForMember(dest => dest.Currencies, opt => opt.MapFrom(src =>
                src.Currencies != null
                    ? src.Currencies.Select(r => new CurrencyResponse
                    {
                        Id = r.Currency.Id,
                        Title = r.Currency.Title,
                        Code = r.Currency.Code,
                        Symbol = r.Currency.Symbol,
                        TitleEn = r.Currency.TitleEn,
                        IsActive = r.Currency.IsActive
                    }).ToList()
                    : new List<CurrencyResponse>()));
    }

    private void CreateCurrencyMappings()
    {
        CreateMap<Currency, CurrencyResponse>()
            .ForMember(dest => dest.Countries, opt => opt.MapFrom(src =>
                src.Countries != null
                    ? src.Countries.Select(r => new CountryResponse
                    {
                        Id = r.Currency.Id,
                        Title = r.Currency.Title,
                        Code = r.Currency.Code,
                        TitleEn = r.Currency.TitleEn,
                        IsActive = r.Currency.IsActive
                    }).ToList()
                    : new List<CountryResponse>()));
    }
}