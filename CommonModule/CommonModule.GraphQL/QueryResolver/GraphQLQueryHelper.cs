using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.GraphQL.Types.Common;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuthGateway.Users;
using CommonModule.GraphQL.Types.Responses.Dictionaries;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Categories;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Countries;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Currencies;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Expenses;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects;
using CommonModule.GraphQL.Types.Responses.Lists;
using CommonModule.GraphQL.Types.Responses.Localizations.Models.Locales;
using CommonModule.Shared.Responses.Auth;
using CommonModule.Shared.Responses.AuthGateway.Users;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Dictionaries.Mediatr.Mediatr.Requests;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using GraphQL.Types;
using Localizations.Mediatr.Mediatr.Locations.Requests;

namespace CommonModule.GraphQL.QueryResolver;

public class GraphQLQueryHelper: GraphQLQueryResolver
{
    public void AddMonolithQueries()
    {
        this.AddLocalizationsQueries();
        this.AddExpensesQueries();
        this.AddDictionariesQueries();
        this.AddAuthGatewayQueries();
    }
    public void AddLocalizationsQueries()
    {
        this.GetVersionedList<
            VersionedListOfGenericType<LocaleResponse, LocaleResponseType>, 
            LocaleResponse, 
            GetLocalesRequest
        >(GraphQLEndpoints.GetLocalesDictionary);
        
        this.GetLocalizations(GraphQLEndpoints.GetLocalizations);
        this.GetLocalizations(GraphQLEndpoints.GetPublicLocalizations);
    }

    public void AddExpensesQueries()
    {
        this.GetFilteredEntities<FilteredListResponseOfGenericType<ExpenseResponse, ExpenseResponseType>, ExpenseResponse, GetFilteredExpensesRequest>(GraphQLEndpoints.GetFilteredExpenses);
        this.GetFilteredEntities<FilteredListResponseOfGenericType<PlannedExpenseResponse, PlannedExpenseResponseType>, PlannedExpenseResponse, GetFilteredPlannedExpensesRequest>(GraphQLEndpoints.GetFilteredPlannedExpenses);
        this.GetFilteredEntities<FilteredListResponseOfGenericType<UserProjectResponse, UserProjectResponseType>, UserProjectResponse, GetFilteredUserProjectsRequest>(GraphQLEndpoints.GetFilteredUserProjects);
        this.GetFilteredEntities<FilteredListResponseOfGenericType<UserAllowedProjectResponse, UserAllowedProjectResponseType>, UserAllowedProjectResponse, GetFilteredUserAllowedProjectsRequest>(GraphQLEndpoints.GetFilteredUserAllowedProjects);
        
        this.GetEntityById<GuidGraphType, UserProjectResponseType, Guid, UserProjectResponse, GetUserProjectByIdRequest, UserProjectResponse>(GraphQLEndpoints.GetUserProjectById);
    }

    public void AddDictionariesQueries()
    {
        this.GetResultForEmptyCommand<
            SiteSettingsResponseType,
            SiteSettingsResponse,
            GetSiteSettingsRequest,
            SiteSettingsResponse
        >(GraphQLEndpoints.SiteSettings);

        this.GetVersionedList<
            VersionedListOfGenericType<CurrencyResponse, CurrencyResponseType>, 
            CurrencyResponse, 
            GetCurrenciesRequest
        >(GraphQLEndpoints.GetCurrenciesDictionary);
        
        this.GetVersionedList<
            VersionedListOfGenericType<CountryResponse, CountryResponseType>, 
            CountryResponse, 
            GetCountriesRequest
        >(GraphQLEndpoints.GetCountriesDictionary);
        
        this.GetVersionedList<
            VersionedListOfGenericType<FrequencyResponse, FrequencyResponseType>, 
            FrequencyResponse, 
            GetFrequenciesRequest
        >(GraphQLEndpoints.GetFrequenciesDictionary);
        
        this.GetVersionedTreeList<
            VersionedListOfGenericType<TreeNodeResponse<CategoryResponse>, TreeNodeResponseOfGenericType<CategoryResponse, CategoryResponseType>>, 
            CategoryResponse, 
            GetCategoriesRequest
        >(GraphQLEndpoints.GetCategoriesDictionary);
    }

    public void AddAuthGatewayQueries()
    {
        this.GetResultForEmptyCommand<
            UserResponseType,
            UserResponse,
            GetCurrentUserRequest,
            UserResponse
        >(GraphQLEndpoints.CurrentUser);

        this.GetResultForNonEmptyCommand<
            AuthSignInRequestInputType,
            JwtTokenResponseType,
            JwtTokenResponse,
            AuthSignInRequest,
            JwtTokenResponse
        >(GraphQLEndpoints.SignIn);
        
        this.ExecuteForEmptyCommand<AuthSignOutRequest, bool>(GraphQLEndpoints.SignOut);
    }
}