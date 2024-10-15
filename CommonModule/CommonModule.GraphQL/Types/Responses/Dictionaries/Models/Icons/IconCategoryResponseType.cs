using CommonModule.Shared.Responses.Dictionaries.Models.Icons;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Icons;

public class IconCategoryResponseType : ObjectGraphType<IconCategoryResponse>
{
    public IconCategoryResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title, nullable: true);
        Field(x => x.IsActive);
        Field(x => x.Icons, type: typeof(ListGraphType<IconResponseType>), nullable: true);
    }
}