using CommonModule.GraphQL.MutationResolver;

namespace Expenses.GraphQL;

public class ExpensesRootMutation: GraphQlMutationHelper
{
    public ExpensesRootMutation()
    {
        Name = "Mutation";
        this.AddExpensesMutations();
    }
}