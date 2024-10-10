using CommonModule.GraphQL;
using CommonModule.GraphQL.QueryResolver;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects;
using CommonModule.GraphQL.Types.Responses.Lists;
using CommonModule.Shared;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using GraphQL.Types;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Dictionaries.GraphQL;

public class ExpensesRootQuery: GraphQLQueryHelper
{
    public ExpensesRootQuery()
    {
        this.AddExpensesQueries();
    }
}  