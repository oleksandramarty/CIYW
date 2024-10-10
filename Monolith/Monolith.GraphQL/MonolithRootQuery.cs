using CommonModule.GraphQL.QueryResolver;

namespace Monolith.GraphQL;

public class MonolithRootQuery : GraphQLQueryHelper
{
    public MonolithRootQuery()
    {
        this.AddMonolithQueries();
    }
}