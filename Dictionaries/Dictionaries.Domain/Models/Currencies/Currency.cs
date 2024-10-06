using System.Text.Json.Serialization;
using CommonModule.Core.JsonConverter;
using CommonModule.Core.Utils;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using Dictionaries.Domain.Models.Countries;

namespace Dictionaries.Domain.Models.Currencies;

public class Currency: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string Symbol { get; set; }
    public string TitleEn { get; set; }
    [JsonConverter(typeof(BooleanJsonConverter))]
    public bool IsActive { get; set; }
    
    [JsonIgnore]
    public ICollection<CountryCurrency> Countries { get; set; }
}