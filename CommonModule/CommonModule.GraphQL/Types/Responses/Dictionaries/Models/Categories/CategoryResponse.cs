using CommonModule.GraphQL.Types.Common;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Categories;

public class CategoryResponseType : ObjectGraphType<CategoryResponse>
{
    public CategoryResponseType()
    {
        Field(x => x.Id);
        Field(x => x.Title);
        Field(x => x.Icon);
        Field(x => x.Color);
        Field(x => x.IsActive);
        Field(x => x.IsPositive);
        Field(x => x.ParentId, nullable: true);
        Field(x => x.Children, type: typeof(ListGraphType<CategoryResponseType>), nullable: true);

    }
}