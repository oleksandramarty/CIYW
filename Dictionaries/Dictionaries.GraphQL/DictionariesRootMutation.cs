using CommonModule.GraphQL.MutationResolver;

namespace Dictionaries.GraphQL;

public class DictionariesRootMutation: GraphQlMutationHelper
{
    public DictionariesRootMutation()
    {
        Name = "Mutation";
    }
}