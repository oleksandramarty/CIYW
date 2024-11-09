using System.Text.Json.Serialization;
using CommonModule.Shared.JsonConvertors;

namespace CommonModule.Shared.Common.BaseInterfaces;

public interface IActivatableEntity
{
    [JsonConverter(typeof(BooleanJsonConverter))]
    bool IsActive { get; set; }
}