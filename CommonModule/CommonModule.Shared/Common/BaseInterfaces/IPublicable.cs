using System.Text.Json.Serialization;
using CommonModule.Shared.JsonConvertors;

namespace CommonModule.Shared.Common.BaseInterfaces;

public interface IPublicable
{
    [JsonConverter(typeof(BooleanJsonConverter))]
    bool IsPublic { get; set; }
}