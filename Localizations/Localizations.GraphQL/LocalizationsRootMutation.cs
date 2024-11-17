using CommonModule.GraphQL.MutationResolver;

namespace Localizations.GraphQL;

public class LocalizationsRootMutation: GraphQlMutationHelper
{
    public LocalizationsRootMutation()
    {
        Name = "Mutation";
    }
}