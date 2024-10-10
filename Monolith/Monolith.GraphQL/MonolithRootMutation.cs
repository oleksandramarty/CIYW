using CommonModule.GraphQL.MutationResolver;

namespace Monolith.GraphQL;

public class MonolithRootMutation: GraphQLMutationHelper
{
    public MonolithRootMutation()
    {
        Name = "Mutation";
        this.AddMonolithMutations();
    }
}