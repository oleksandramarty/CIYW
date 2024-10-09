using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Localizations.GraphQL;

public class LocalizationsGraphQLSchema: Schema
{
    public LocalizationsGraphQLSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<LocalizationsRootQuery>();
        // Mutation = serviceProvider.GetRequiredService<DictionariesRootMutation>();
    }
}