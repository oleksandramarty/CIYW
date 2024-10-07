using CommonModule.GraphQL;
using CommonModule.GraphQL.MutationResolver;
using CommonModule.GraphQL.Types.InputTypes.Expenses;
using CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;
using CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using GraphQL.Types;

namespace Expenses.GraphQL;

public class ExpensesRootMutation: GraphQLMutationResolver
{
    public ExpensesRootMutation()
    {
        Name = "Mutation";
        this.CreateEntity<CreateOrUpdateExpenseInputType, CreateOrUpdateExpenseCommand>(GraphQLEndpoints.CreateExpense);
        this.UpdateEntity<CreateOrUpdateExpenseInputType, CreateOrUpdateExpenseCommand, GuidGraphType>(GraphQLEndpoints.UpdateExpense);
        this.DeleteEntity<RemoveExpenseCommand, GuidGraphType>(GraphQLEndpoints.RemoveExpense);
        
        this.CreateEntity<CreateOrUpdatePlannedExpenseInputType, CreateOrUpdatePlannedExpenseCommand>(GraphQLEndpoints.CreatePlannedExpense);
        this.UpdateEntity<CreateOrUpdatePlannedExpenseInputType, CreateOrUpdatePlannedExpenseCommand, GuidGraphType>(GraphQLEndpoints.UpdatePlannedExpense);
        this.DeleteEntity<RemovePlannedExpenseCommand, GuidGraphType>(GraphQLEndpoints.RemovePlannedExpense);
        
        this.CreateEntity<CreateUserProjectInputType, CreateUserProjectCommand>(GraphQLEndpoints.CreateUserProject);
    }
}