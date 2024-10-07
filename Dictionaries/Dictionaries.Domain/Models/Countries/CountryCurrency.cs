using CommonModule.Shared.Common;

namespace Dictionaries.Domain.Models.Countries;

public class CountryCurrency
{
    public int CountryId { get; set; }
    public Country Country { get; set; }
    
    public int CurrencyId { get; set; }
    public Currencies.Currency Currency { get; set; }
}