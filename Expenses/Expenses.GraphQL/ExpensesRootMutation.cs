using CommonModule.GraphQL.MutationResolver;

namespace Expenses.GraphQL;

public class ExpensesRootMutation: GraphQLMutationHelper
{
    public ExpensesRootMutation()
    {
        Name = "Mutation";
        this.AddExpensesMutations();
    }
}