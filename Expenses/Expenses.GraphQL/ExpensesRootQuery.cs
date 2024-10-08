using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects;
using CommonModule.GraphQL.Types.Responses.ListWithInclude;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionaries.GraphQL;

public class ExpensesRootQuery: GraphQLQueryResolver
{
    public ExpensesRootQuery()
    {
        this.GetFilteredEntities<FilteredListResponseOfGenericType<ExpenseResponse, ExpenseResponseType>, ExpenseResponse, GetFilteredExpensesRequest>(GraphQLEndpoints.GetFilteredExpenses);
        this.GetFilteredEntities<FilteredListResponseOfGenericType<PlannedExpenseResponse, PlannedExpenseResponseType>, PlannedExpenseResponse, GetFilteredPlannedExpensesRequest>(GraphQLEndpoints.GetFilteredPlannedExpenses);
        this.GetFilteredEntities<FilteredListResponseOfGenericType<UserProjectResponse, UserProjectResponseType>, UserProjectResponse, GetFilteredUserProjectsRequest>(GraphQLEndpoints.GetFilteredUserProjects);
        this.GetFilteredEntities<FilteredListResponseOfGenericType<UserAllowedProjectResponse, UserAllowedProjectResponseType>, UserAllowedProjectResponse, GetFilteredUserAllowedProjectsRequest>(GraphQLEndpoints.GetFilteredUserAllowedProjects);
        
        this.GetEntityById<GuidGraphType, UserProjectResponseType, Guid, UserProjectResponse, GetUserProjectByIdRequest, UserProjectResponse>(GraphQLEndpoints.GetUserProjectById);
    }
}  