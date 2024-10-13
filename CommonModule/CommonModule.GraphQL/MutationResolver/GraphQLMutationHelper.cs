using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway.Users;
using CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;
using CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Mediatr.Projects.Handlers;
using GraphQL.Types;

namespace CommonModule.GraphQL.MutationResolver;

public class GraphQLMutationHelper: GraphQLMutationResolver
{
    public void AddMonolithMutations()
    {
        this.AddExpensesMutations();
        this.AddAuthGatewayMutations();
    }
    public void AddExpensesMutations()
    {
        this.CreateEntity<CreateExpenseInputType, CreateExpenseCommand>(GraphQLEndpoints.CreateExpense);
        this.UpdateEntity<UpdateExpenseInputType, GuidGraphType, Guid, UpdateExpenseCommand>(GraphQLEndpoints.UpdateExpense);
        this.DeleteEntity<RemoveExpenseCommand, GuidGraphType, Guid>(GraphQLEndpoints.RemoveExpense);
        
        this.CreateEntity<CreatePlannedExpenseInputType, CreatePlannedExpenseCommand>(GraphQLEndpoints.CreatePlannedExpense);
        this.UpdateEntity<UpdatePlannedExpenseInputType, GuidGraphType, Guid, UpdatePlannedExpenseCommand>(GraphQLEndpoints.UpdatePlannedExpense);
        this.DeleteEntity<RemovePlannedExpenseCommand, GuidGraphType, Guid>(GraphQLEndpoints.RemovePlannedExpense);
        
        this.CreateEntity<CreateUserProjectInputType, CreateUserProjectCommand>(GraphQLEndpoints.CreateUserProject);
        this.CreateEntity<UpdateUserProjectInputType, UpdateUserProjectCommand>(GraphQLEndpoints.UpdateUserProject);
        
        this.CreateEntity<CreateUserBalanceInputType, CreateUserBalanceCommand>(GraphQLEndpoints.CreateUserBalance);
        this.UpdateEntity<UpdateUserBalanceInputType, GuidGraphType, Guid, UpdateUserBalanceCommand>(GraphQLEndpoints.UpdateUserBalance);
        this.DeleteEntity<RemoveUserBalanceCommand, GuidGraphType, Guid>(GraphQLEndpoints.RemoveUserBalance);
    }

    public void AddAuthGatewayMutations()
    {
        this.CreateEntity<CreateOrUpdateUserSettingsInputType, CreateUserSettingCommand>(GraphQLEndpoints.CreateUserSettings);
        this.UpdateEntity<CreateOrUpdateUserSettingsInputType, GuidGraphType, Guid, UpdateUserSettingCommand>(GraphQLEndpoints.UpdateUserSettings);

        this.CreateEntity<AuthSignUpInputType, AuthSignUpCommand>(GraphQLEndpoints.SignUp);
    }
}