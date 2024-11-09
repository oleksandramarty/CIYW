using CommonModule.GraphQL.QueryResolver;

namespace Dictionaries.GraphQL;

public class DictionariesRootQuery : GraphQLQueryHelper
{
    public DictionariesRootQuery()
    {
        this.AddDictionariesQueries();
    }
}