using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace CommonModule.Shared.Responses.Dictionaries.Models.Icons;

public class IconCategoryResponse: BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<IconResponse> Icons { get; set; }
}