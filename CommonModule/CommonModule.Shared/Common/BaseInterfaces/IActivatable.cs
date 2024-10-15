using System.Text.Json.Serialization;
using CommonModule.Shared.JsonConvertors;

namespace CommonModule.Shared.Common.BaseInterfaces;

public interface IActivatable
{
    [JsonConverter(typeof(BooleanJsonConverter))]
    bool IsActive { get; set; }
}