using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;

namespace Dictionaries.Domain.Models.Icons;

public class IconCategory : BaseIdEntity<int>, IActivatable
{
    public string Title { get; set; }
    public bool IsActive { get; set; }
    
    public ICollection<Icon> Icons { get; set; }
}