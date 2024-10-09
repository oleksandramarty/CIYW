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

public class ExpensesRootMutation: GraphQLMutationResolver
{
    public ExpensesRootMutation()
    {
        Name = "Mutation";
        this.CreateEntity<CreateExpenseInputType, CreateExpenseCommand>(GraphQLEndpoints.CreateExpense);
        this.UpdateEntity<UpdateExpenseInputType, GuidGraphType, Guid, UpdateExpenseCommand>(GraphQLEndpoints.UpdateExpense);
        this.DeleteEntity<RemoveExpenseCommand, GuidGraphType, Guid>(GraphQLEndpoints.RemoveExpense);
        
        this.CreateEntity<CreatePlannedExpenseInputType, CreatePlannedExpenseCommand>(GraphQLEndpoints.CreatePlannedExpense);
        this.UpdateEntity<UpdatePlannedExpenseInputType, GuidGraphType, Guid, UpdatePlannedExpenseCommand>(GraphQLEndpoints.UpdatePlannedExpense);
        this.DeleteEntity<RemovePlannedExpenseCommand, GuidGraphType, Guid>(GraphQLEndpoints.RemovePlannedExpense);
        
        this.CreateEntity<CreateUserProjectInputType, CreateUserProjectCommand>(GraphQLEndpoints.CreateUserProject);
    }
}