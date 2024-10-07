using CommonModule.GraphQL.MutationResolver;

namespace Dictionaries.GraphQL;

public class DictionariesRootMutation: GraphQLMutationResolver
{
    public DictionariesRootMutation()
    {
        Name = "Mutation";
    }
}