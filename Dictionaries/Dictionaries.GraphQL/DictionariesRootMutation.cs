using CommonModule.GraphQL.MutationResolver;

namespace Dictionaries.GraphQL;

public class DictionariesRootMutation: GraphQLMutationHelper
{
    public DictionariesRootMutation()
    {
        Name = "Mutation";
    }
}