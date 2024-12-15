using AuthGateway.Mediatr.Mediatr.Auth.Commands;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway.Users;
using CommonModule.GraphQL.Types.InputTypes.Expenses.Expenses;
using CommonModule.GraphQL.Types.InputTypes.Expenses.UserProjects;
using CommonModule.GraphQL.Types.Responses.Base;
using CommonModule.Shared.Responses.Base;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using GraphQL.Types;

namespace CommonModule.GraphQL.MutationResolver;

public class GraphQlMutationHelper: GraphQlMutationResolver
{
    public void AddMonolithMutations()
    {
        AddExpensesMutations();
        AddAuthGatewayMutations();
        AddAuditTrailMutations();
    }
    public void AddAuditTrailMutations()
    {
    }
    public void AddExpensesMutations()
    {
        CreateEntity<CreateExpenseInputType, BaseEntityIdOfGuidResponseType, CreateExpenseCommand, BaseEntityIdResponse<Guid>>(GraphQlEndpoints.CreateExpense);
        UpdateEntity<UpdateExpenseInputType, GuidGraphType, Guid, UpdateExpenseCommand>(GraphQlEndpoints.UpdateExpense);
        DeleteEntity<RemoveExpenseCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemoveExpense);
        
        CreateEntity<CreatePlannedExpenseInputType, BaseEntityIdOfGuidResponseType, CreatePlannedExpenseCommand, BaseEntityIdResponse<Guid>>(GraphQlEndpoints.CreatePlannedExpense);
        UpdateEntity<UpdatePlannedExpenseInputType, GuidGraphType, Guid, UpdatePlannedExpenseCommand>(GraphQlEndpoints.UpdatePlannedExpense);
        DeleteEntity<RemovePlannedExpenseCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemovePlannedExpense);
        
        CreateEntity<CreateFavoriteExpenseInputType, BaseEntityIdOfGuidResponseType, CreateFavoriteExpenseCommand, BaseEntityIdResponse<Guid>>(GraphQlEndpoints.CreateFavoriteExpense);
        UpdateEntity<UpdateFavoriteExpenseInputType, GuidGraphType, Guid, UpdateFavoriteExpenseCommand>(GraphQlEndpoints.UpdateFavoriteExpense);
        DeleteEntity<RemoveFavoriteExpenseCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemoveFavoriteExpense);
        
        CreateEntity<CreateUserProjectInputType, BaseEntityIdOfGuidResponseType, CreateUserProjectCommand, BaseEntityIdResponse<Guid>>(GraphQlEndpoints.CreateUserProject);
        UpdateEntity<UpdateUserProjectInputType, GuidGraphType, Guid, UpdateUserProjectCommand>(GraphQlEndpoints.UpdateUserProject);
        
        CreateEntity<CreateUserBalanceInputType, BaseEntityIdOfGuidResponseType, CreateUserBalanceCommand, BaseEntityIdResponse<Guid>>(GraphQlEndpoints.CreateUserBalance);
        UpdateEntity<UpdateUserBalanceInputType, GuidGraphType, Guid, UpdateUserBalanceCommand>(GraphQlEndpoints.UpdateUserBalance);
        DeleteEntity<RemoveUserBalanceCommand, GuidGraphType, Guid>(GraphQlEndpoints.RemoveUserBalance);
    }

    public void AddAuthGatewayMutations()
    {
        CreateEntity<CreateOrUpdateUserSettingsInputType, BaseEntityIdOfGuidResponseType, CreateUserSettingCommand, BaseEntityIdResponse<Guid>>(GraphQlEndpoints.CreateUserSettings);
        UpdateEntity<CreateOrUpdateUserSettingsInputType, GuidGraphType, Guid, UpdateUserSettingCommand>(GraphQlEndpoints.UpdateUserSettings);

        CreateEntity<AuthSignUpInputType, BaseEntityIdOfGuidResponseType, AuthSignUpCommand, BaseEntityIdResponse<Guid>>(GraphQlEndpoints.SignUp);
    }
}