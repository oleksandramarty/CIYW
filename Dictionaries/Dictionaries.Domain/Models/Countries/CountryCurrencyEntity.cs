using CommonModule.Shared.Common;

namespace Dictionaries.Domain.Models.Countries;

public class CountryCurrencyEntity
{
    public int CountryId { get; set; }
    public CountryEntity Country { get; set; }
    
    public int CurrencyId { get; set; }
    public Currencies.CurrencyEntity Currency { get; set; }
}