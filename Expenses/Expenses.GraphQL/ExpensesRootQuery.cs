using CommonModule.GraphQL.QueryResolver;

namespace Expenses.GraphQL;

public class ExpensesRootQuery: GraphQLQueryHelper
{
    public ExpensesRootQuery()
    {
        this.AddExpensesQueries();
    }
}  