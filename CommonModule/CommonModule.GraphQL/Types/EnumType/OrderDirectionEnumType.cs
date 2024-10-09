using CommonModule.Shared.Enums;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.EnumType;

public class OrderDirectionEnumType: EnumerationGraphType<OrderDirectionEnum>
{
    public OrderDirectionEnumType()
    {
        Name = "OrderDirection";
        Description = "The order direction for sorting.";
    }
}