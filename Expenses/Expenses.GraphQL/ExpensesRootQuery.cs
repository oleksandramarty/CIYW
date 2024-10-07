using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
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
        this.GetFilteredEntities<ListWithIncludeResponseOfExpensesType, ExpenseResponse, GetFilteredExpensesRequest>(GraphQLEndpoints.GetFilteredExpenses);
        this.GetFilteredEntities<ListWithIncludeResponseOfPlannedExpensesType, PlannedExpenseResponse, GetFilteredPlannedExpensesRequest>(GraphQLEndpoints.GetFilteredPlannedExpenses);
        
        this.GetEntityById<ScalarGraphType, UserProjectResponseType, Guid, GetUserProjectByIdRequest, UserProjectResponse>(GraphQLEndpoints.GetUserProjectById);

        this.GetResultsForEmptyCommand<UserProjectResponseType, UserProjectResponse, GetUserProjectsRequest, List<UserProjectResponse>>(GraphQLEndpoints.GetUserProjects);
        this.GetResultsForEmptyCommand<UserAllowedProjectResponseType, UserAllowedProjectResponse, GetUserAllowedProjectsRequest, List<UserAllowedProjectResponse>>(GraphQLEndpoints.GetUserAllowedProjects);
    }
}  