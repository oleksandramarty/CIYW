using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Localizations;

public class LocalizationsResponse: BaseVersionEntity
{
    public Dictionary<string, Dictionary<string, string>> Data { get; set; }
}