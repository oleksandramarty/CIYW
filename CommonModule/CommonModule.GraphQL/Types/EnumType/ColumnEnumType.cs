using CommonModule.Shared.Enums;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.EnumType;

public class ColumnEnumType: EnumerationGraphType<ColumnEnum>
{
    public ColumnEnumType()
    {
        Name = "Column";
        Description = "The columns available for sorting.";
    }
}