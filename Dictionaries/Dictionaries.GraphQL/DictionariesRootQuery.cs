using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
using CommonModule.GraphQL.Types.Common;
using CommonModule.GraphQL.Types.Responses.Dictionaries;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Categories;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Countries;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Currencies;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Expenses;
using CommonModule.GraphQL.Types.Responses.Lists;
using CommonModule.Shared;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using Dictionaries.Mediatr.Mediatr.Requests;

namespace Dictionaries.GraphQL;

public class DictionariesRootQuery : GraphQLQueryHelper
{
    public DictionariesRootQuery()
    {
        this.AddDictionariesQueries();
    }
}