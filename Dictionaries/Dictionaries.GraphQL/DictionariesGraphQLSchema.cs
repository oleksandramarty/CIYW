using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionaries.GraphQL;

public class DictionariesGraphQLSchema: Schema
{
    public DictionariesGraphQLSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<DictionariesRootQuery>();
        // Mutation = serviceProvider.GetRequiredService<DictionariesRootMutation>();
    }
}