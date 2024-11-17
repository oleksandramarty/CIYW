using AuditTrail.Mediatr.Mediatr.Requests;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CommonModule.GraphQL.Types.Common;
using CommonModule.GraphQL.Types.InputTypes.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuditTrail.AuditTrail;
using CommonModule.GraphQL.Types.Responses.AuthGateway;
using CommonModule.GraphQL.Types.Responses.AuthGateway.Users;
using CommonModule.GraphQL.Types.Responses.Base;
using CommonModule.GraphQL.Types.Responses.Dictionaries;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Balances;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Categories;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Countries;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Currencies;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Expenses;
using CommonModule.GraphQL.Types.Responses.Dictionaries.Models.Icons;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Expenses;
using CommonModule.GraphQL.Types.Responses.Expenses.Models.Projects;
using CommonModule.GraphQL.Types.Responses.Lists;
using CommonModule.GraphQL.Types.Responses.Localizations.Models.Locales;
using CommonModule.Shared.Responses.AuditTrail;
using CommonModule.Shared.Responses.Auth;
using CommonModule.Shared.Responses.AuthGateway.Users;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using CommonModule.Shared.Responses.Dictionaries.Models.Icons;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using Dictionaries.Mediatr.Mediatr.Requests;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using GraphQL.Types;
using Localizations.Mediatr.Mediatr.Locations.Requests;

namespace CommonModule.GraphQL.QueryResolver;

public class GraphQlQueryHelper: GraphQlQueryResolver
{
    public void AddMonolithQueries()
    {
        this.AddLocalizationsQueries();
        this.AddExpensesQueries();
        this.AddDictionariesQueries();
        this.AddAuthGatewayQueries();
        this.AddAuditTrailQueries();
    }
    public void AddLocalizationsQueries()
    {
        this.VersionedList<
            VersionedListOfGenericType<LocaleResponse, LocaleResponseType>, 
            LocaleResponse, 
            LocalesRequest
        >(GraphQlEndpoints.LocalesDictionary);
        
        this.Localizations(GraphQlEndpoints.Localizations);
        this.Localizations(GraphQlEndpoints.PublicLocalizations);
    }

    public void AddExpensesQueries()
    {
        this.FilteredEntities<FilteredListResponseOfGenericType<ExpenseResponse, ExpenseResponseType>, ExpenseResponse, FilteredExpensesRequest>(GraphQlEndpoints.FilteredExpenses);
        this.FilteredEntities<FilteredListResponseOfGenericType<PlannedExpenseResponse, PlannedExpenseResponseType>, PlannedExpenseResponse, FilteredPlannedExpensesRequest>(GraphQlEndpoints.FilteredPlannedExpenses);
        this.FilteredEntities<FilteredListResponseOfGenericType<FavoriteExpenseResponse, FavoriteExpenseResponseType>, FavoriteExpenseResponse, FilteredFavoriteExpensesRequest>(GraphQlEndpoints.FilteredFavoriteExpenses);
        this.FilteredEntities<FilteredListResponseOfGenericType<UserProjectResponse, UserProjectResponseType>, UserProjectResponse, FilteredUserProjectsRequest>(GraphQlEndpoints.FilteredUserProjects);
        this.FilteredEntities<FilteredListResponseOfGenericType<UserAllowedProjectResponse, UserAllowedProjectResponseType>, UserAllowedProjectResponse, FilteredUserAllowedProjectsRequest>(GraphQlEndpoints.FilteredUserAllowedProjects);
        
        this.EntityById<GuidGraphType, UserProjectResponseType, Guid, UserProjectResponse, UserProjectByIdRequest, UserProjectResponse>(GraphQlEndpoints.UserProjectById);
    }
    public void AddAuditTrailQueries()
    {
        this.FilteredEntities<FilteredListResponseOfGenericType<AuditTrailResponse, AuditTrailResponseType>, AuditTrailResponse, FilteredAuditTrailRequest>(GraphQlEndpoints.FilteredAuditTrail);
    }

    public void AddDictionariesQueries()
    {
        this.ResultForEmptyCommand<
            SiteSettingsResponseType,
            SiteSettingsResponse,
            SiteSettingsRequest,
            SiteSettingsResponse
        >(GraphQlEndpoints.SiteSettings);

        this.VersionedList<
            VersionedListOfGenericType<CurrencyResponse, CurrencyResponseType>, 
            CurrencyResponse, 
            CurrenciesRequest
        >(GraphQlEndpoints.CurrenciesDictionary);
        
        this.VersionedList<
            VersionedListOfGenericType<CountryResponse, CountryResponseType>, 
            CountryResponse, 
            CountriesRequest
        >(GraphQlEndpoints.CountriesDictionary);
        
        this.VersionedList<
            VersionedListOfGenericType<FrequencyResponse, FrequencyResponseType>, 
            FrequencyResponse, 
            FrequenciesRequest
        >(GraphQlEndpoints.FrequenciesDictionary);
        
        this.VersionedList<
            VersionedListOfGenericType<CategoryResponse, CategoryResponseType>, 
            CategoryResponse, 
            CategoriesRequest
        >(GraphQlEndpoints.CategoriesDictionary);
        
        this.VersionedList<
            VersionedListOfGenericType<BalanceTypeResponse, BalanceTypeResponseType>, 
            BalanceTypeResponse, 
            BalanceTypesRequest
        >(GraphQlEndpoints.BalanceTypesDictionary);
        
        this.VersionedList<
            VersionedListOfGenericType<IconCategoryResponse, IconCategoryResponseType>, 
            IconCategoryResponse, 
            IconCategoriesRequest
        >(GraphQlEndpoints.IconCategoriesDictionary);
    }

    public void AddAuthGatewayQueries()
    {
        this.ResultForEmptyCommand<
            UserResponseType,
            UserResponse,
            CurrentUserRequest,
            UserResponse
        >(GraphQlEndpoints.CurrentUser);

        this.ResultForNonEmptyCommand<
            AuthSignInRequestInputType,
            JwtTokenResponseType,
            JwtTokenResponse,
            AuthSignInRequest,
            JwtTokenResponse
        >(GraphQlEndpoints.SignIn);
        
        this.ExecuteForEmptyCommand<BaseBoolResponseType, AuthSignOutRequest, BaseBoolResponse>(GraphQlEndpoints.SignOut);
    }
}