using CommonModule.GraphQL.MutationResolver;

namespace Localizations.GraphQL;

public class LocalizationsRootMutation: GraphQLMutationHelper
{
    public LocalizationsRootMutation()
    {
        Name = "Mutation";
    }
}