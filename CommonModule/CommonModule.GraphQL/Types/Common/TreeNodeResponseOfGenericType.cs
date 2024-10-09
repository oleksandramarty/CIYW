using CommonModule.Shared.Responses.Base;
using GraphQL.Types;

namespace CommonModule.GraphQL.Types.Common;

public class TreeNodeResponseOfGenericType<TEntityResponse, TEntityResponseType> : ObjectGraphType<TreeNodeResponse<TEntityResponse>>
    where TEntityResponseType: ObjectGraphType<TEntityResponse>
{
    public TreeNodeResponseOfGenericType()
    {
        Field(x => x.Node, type: typeof(TEntityResponseType));
        Field(x => x.Parent, type: typeof(TEntityResponseType), nullable: true);
    }
}