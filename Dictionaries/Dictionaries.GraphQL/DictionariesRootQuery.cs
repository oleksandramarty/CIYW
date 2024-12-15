using CommonModule.GraphQL.QueryResolver;

namespace Dictionaries.GraphQL;

public class DictionariesRootQuery : GraphQlQueryHelper
{
    public DictionariesRootQuery()
    {
        AddDictionariesQueries();
    }
}