using CommonModule.GraphQL.QueryResolver;

namespace Expenses.GraphQL;

public class ExpensesRootQuery: GraphQlQueryHelper
{
    public ExpensesRootQuery()
    {
        this.AddExpensesQueries();
    }
}  