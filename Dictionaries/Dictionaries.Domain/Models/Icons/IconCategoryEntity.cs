using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CommonModule.Shared.Common;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;

namespace Dictionaries.Domain.Models.Icons;

public class IconCategoryEntity : BaseIdEntity<int>, IStatusEntity
{
    [Required] [MaxLength(100)] public required string Title { get; set; }
    public StatusEnum Status { get; set; }

    public ICollection<IconEntity> Icons { get; set; }
}