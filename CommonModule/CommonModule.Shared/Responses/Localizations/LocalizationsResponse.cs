using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Localizations;

public class LocalizationsResponse: BaseVersionEntity
{
    public LocalizationsResponse()
    {
        this.Data = new List<LocalizationResponse>();
    }
    public ICollection<LocalizationResponse> Data { get; set; }
}

public class LocalizationResponse
{
    public LocalizationResponse()
    {
        this.Items = new List<LocalizationItemResponse>();
    }
    
    public LocalizationResponse(string locale)
    {
        this.Locale = locale;
        this.Items = new List<LocalizationItemResponse>();
    }
    public string Locale { get; set; }
    public ICollection<LocalizationItemResponse> Items { get; set; }
}

public class LocalizationItemResponse
{
    public LocalizationItemResponse()
    {
        
    }
    
    public LocalizationItemResponse(string key, string value)
    {
        this.Key = key;
        this.Value = value;
    }
    
    public string Key { get; set; }
    public string Value { get; set; }
}