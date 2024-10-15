using System.Text.Json.Serialization;
using CommonModule.Core.JsonConverter;
using CommonModule.Shared.Common;
 using CommonModule.Shared.Common.BaseInterfaces;
 using CommonModule.Shared.JsonConvertors;

 namespace Dictionaries.Domain.Models.Countries;
 
 public class Country: BaseIdEntity<int>, IActivatable
 {
     public string Title { get; set; }
     public string Code { get; set; }
     public string TitleEn { get; set; }
     [JsonConverter(typeof(BooleanJsonConverter))]
     public bool IsActive { get; set; }
     
     [JsonIgnore]
     public ICollection<CountryCurrency> Currencies { get; set; }
 }