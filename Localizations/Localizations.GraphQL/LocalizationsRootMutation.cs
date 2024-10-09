using CommonModule.GraphQL.MutationResolver;

namespace Localizations.GraphQL;

public class LocalizationsRootMutation: GraphQLMutationResolver
{
    public LocalizationsRootMutation()
    {
        Name = "Mutation";
    }
}