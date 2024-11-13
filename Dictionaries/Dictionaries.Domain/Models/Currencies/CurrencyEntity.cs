using System.Text.Json.Serialization;
using CommonModule.Core.JsonConverter;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.JsonConvertors;
using Dictionaries.Domain.Models.Countries;

namespace Dictionaries.Domain.Models.Currencies;

public class CurrencyEntity: BaseIdEntity<int>, IActivatableEntity
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string Symbol { get; set; }
    public string TitleEn { get; set; }
    [JsonConverter(typeof(BooleanJsonConverter))]
    public bool IsActive { get; set; }
    
    [JsonIgnore]
    public ICollection<CountryCurrencyEntity> Countries { get; set; }
}