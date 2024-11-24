using CommonModule.GraphQL.Types.EnumType;
using CommonModule.Shared.Responses.Dictionaries.Models.Icons;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Icons;

public sealed class IconResponseType : ObjectGraphType<IconResponse>
{
    public IconResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title, nullable: true);
        Field(x => x.Status, type: typeof(StatusEnumType));
        Field(x => x.IconCategoryId);
    }
}