namespace CommonModule.Shared.Responses.Localizations;

public class LocalizationsResponse
{
    public Dictionary<string, Dictionary<string, string>> Data { get; set; }
    public string Version { get; set; }
}