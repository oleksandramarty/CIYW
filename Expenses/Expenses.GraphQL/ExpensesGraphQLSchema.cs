using Dictionaries.GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Expenses.GraphQL;

public class ExpensesGraphQLSchema : Schema
{
    public ExpensesGraphQLSchema(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<ExpensesRootQuery>();
        Mutation = serviceProvider.GetRequiredService<ExpensesRootMutation>();
    }
}