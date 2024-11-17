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

public class GraphQlMutationHelper: GraphQlMutationResolver
{
    public void AddMonolithMutations()
    {
        this.AddExpensesMutations();
        this.AddAuthGatewayMutations();
        this.AddAuditTrailMutations();
    }
    public void AddAuditTrailMutations()
    {
    }
    public void AddExpensesMutations()
    {
        this.CreateEntity<CreateExpenseInputType, CreateExpenseCommand>(GraphQlEndpoints.CreateExpense);
        this.UpdateEntity<UpdateExpenseInputType, GuidGraphType, Guid, UpdateExpenseCommand>(GraphQlEndpoints.UpdateExpense);
        this.DeleteEntity<RemoveExpenseCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemoveExpense);
        
        this.CreateEntity<CreatePlannedExpenseInputType, CreatePlannedExpenseCommand>(GraphQlEndpoints.CreatePlannedExpense);
        this.UpdateEntity<UpdatePlannedExpenseInputType, GuidGraphType, Guid, UpdatePlannedExpenseCommand>(GraphQlEndpoints.UpdatePlannedExpense);
        this.DeleteEntity<RemovePlannedExpenseCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemovePlannedExpense);
        
        this.CreateEntity<CreateFavoriteExpenseInputType, CreateFavoriteExpenseCommand>(GraphQlEndpoints.CreateFavoriteExpense);
        this.UpdateEntity<UpdateFavoriteExpenseInputType, GuidGraphType, Guid, UpdateFavoriteExpenseCommand>(GraphQlEndpoints.UpdateFavoriteExpense);
        this.DeleteEntity<RemoveFavoriteExpenseCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemoveFavoriteExpense);
        
        this.CreateEntity<CreateUserProjectInputType, CreateUserProjectCommand>(GraphQlEndpoints.CreateUserProject);
        this.CreateEntity<UpdateUserProjectInputType, UpdateUserProjectCommand>(GraphQlEndpoints.UpdateUserProject);
        
        this.CreateEntity<CreateUserBalanceInputType, CreateUserBalanceCommand>(GraphQlEndpoints.CreateUserBalance);
        this.UpdateEntity<UpdateUserBalanceInputType, GuidGraphType, Guid, UpdateUserBalanceCommand>(GraphQlEndpoints.UpdateUserBalance);
        this.DeleteEntity<RemoveUserBalanceCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemoveUserBalance);
    }

    public void AddAuthGatewayMutations()
    {
        this.CreateEntity<CreateOrUpdateUserSettingsInputType, CreateUserSettingCommand>(GraphQlEndpoints.CreateUserSettings);
        this.UpdateEntity<CreateOrUpdateUserSettingsInputType, GuidGraphType, Guid, UpdateUserSettingCommand>(GraphQlEndpoints.UpdateUserSettings);

        this.CreateEntity<AuthSignUpInputType, AuthSignUpCommand>(GraphQlEndpoints.SignUp);
    }
}