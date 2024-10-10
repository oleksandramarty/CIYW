using CommonModule.GraphQL;
using CommonModule.GraphQL.MutationResolver;
using CommonModule.GraphQL.Types.InputTypes.Expenses;
using CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;
using CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;
using CommonModule.Shared;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using GraphQL.Types;

namespace Expenses.GraphQL;

public class ExpensesRootMutation: GraphQLMutationHelper
{
    public ExpensesRootMutation()
    {
        Name = "Mutation";
        this.AddExpensesMutations();
    }
}