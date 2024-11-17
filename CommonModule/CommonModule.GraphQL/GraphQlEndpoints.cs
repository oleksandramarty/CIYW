namespace CommonModule.GraphQL;

public static class GraphQlEndpoints
{
    public static readonly GraphQlEndpoint CurrentUser = new GraphQlEndpoint("auth_gateway_current_user");
    public static readonly GraphQlEndpoint SiteSettings = new GraphQlEndpoint("dictionaries_site_settings", false);
    public static readonly GraphQlEndpoint SignIn = new GraphQlEndpoint("auth_gateway_sign_in", false);
    public static readonly GraphQlEndpoint SignOut = new GraphQlEndpoint("auth_gateway_sign_out");
    public static readonly GraphQlEndpoint SignUp = new GraphQlEndpoint("auth_gateway_sign_up", false);
    public static readonly GraphQlEndpoint CreateExpense = new GraphQlEndpoint("expenses_create_expense");
    public static readonly GraphQlEndpoint UpdateExpense = new GraphQlEndpoint("expenses_update_expense");
    public static readonly GraphQlEndpoint CreateUserBalance = new GraphQlEndpoint("expenses_create_user_balance");
    public static readonly GraphQlEndpoint UpdateUserBalance = new GraphQlEndpoint("expenses_update_user_balance");
    public static readonly GraphQlEndpoint CreateFavoriteExpense = new GraphQlEndpoint("expenses_create_favorite_expense");
    public static readonly GraphQlEndpoint UpdateFavoriteExpense = new GraphQlEndpoint("expenses_update_favorite_expense");
    public static readonly GraphQlEndpoint RemoveUserBalance = new GraphQlEndpoint("expenses_remove_user_balance");
    public static readonly GraphQlEndpoint RemoveExpense = new GraphQlEndpoint("expenses_remove_expense");
    public static readonly GraphQlEndpoint RemoveFavoriteExpense = new GraphQlEndpoint("expenses_favorite_remove_expense");
    public static readonly GraphQlEndpoint CreatePlannedExpense = new GraphQlEndpoint("expenses_create_planned_expense");
    public static readonly GraphQlEndpoint UpdatePlannedExpense = new GraphQlEndpoint("expenses_update_planned_expense");
    public static readonly GraphQlEndpoint RemovePlannedExpense = new GraphQlEndpoint("expenses_remove_planned_expense");
    public static readonly GraphQlEndpoint CreateUserProject = new GraphQlEndpoint("expenses_create_user_project");
    public static readonly GraphQlEndpoint UpdateUserProject = new GraphQlEndpoint("expenses_update_user_project");
    public static readonly GraphQlEndpoint FilteredAuditTrail = new GraphQlEndpoint("audit_trail_filtered_audit_trail");
    public static readonly GraphQlEndpoint FilteredExpenses = new GraphQlEndpoint("expenses_filtered_expenses");
    public static readonly GraphQlEndpoint FilteredPlannedExpenses = new GraphQlEndpoint("expenses_filtered_planned_expenses");
    public static readonly GraphQlEndpoint FilteredFavoriteExpenses = new GraphQlEndpoint("expenses_filtered_favorite_expenses");
    public static readonly GraphQlEndpoint UserProjectById = new GraphQlEndpoint("expenses_user_project_by_id");
    public static readonly GraphQlEndpoint FilteredUserProjects = new GraphQlEndpoint("expenses_filtered_user_projects");
    public static readonly GraphQlEndpoint FilteredUserAllowedProjects = new GraphQlEndpoint("expenses_filtered_user_allowed_projects");
    public static readonly GraphQlEndpoint CreateUserSettings = new GraphQlEndpoint("auth_gateway_create_user_settings");
    public static readonly GraphQlEndpoint UpdateUserSettings = new GraphQlEndpoint("auth_gateway_update_user_settings");
    public static readonly GraphQlEndpoint BalanceTypesDictionary = new GraphQlEndpoint("dictionaries_balance_types_dictionary");
    public static readonly GraphQlEndpoint IconCategoriesDictionary = new GraphQlEndpoint("dictionaries_icon_categories_dictionary");
    public static readonly GraphQlEndpoint CurrenciesDictionary = new GraphQlEndpoint("dictionaries_currencies_dictionary");
    public static readonly GraphQlEndpoint CategoriesDictionary = new GraphQlEndpoint("dictionaries_categories_dictionary");
    public static readonly GraphQlEndpoint CountriesDictionary = new GraphQlEndpoint("dictionaries_countries_dictionary");
    public static readonly GraphQlEndpoint LocalesDictionary = new GraphQlEndpoint("localizations_locales_dictionary", false);
    public static readonly GraphQlEndpoint FrequenciesDictionary = new GraphQlEndpoint("dictionaries_frequencies_dictionary");
    public static readonly GraphQlEndpoint PublicLocalizations = new GraphQlEndpoint("localizations_public_localizations", false);
    public static readonly GraphQlEndpoint Localizations = new GraphQlEndpoint("localizations_localizations");
}


public class GraphQlEndpoint
{
    public GraphQlEndpoint(string name, bool isAuthenticated = true)
    {
        Name = name;
        IsAuthenticated = isAuthenticated;
    }
    
    public string Name { get; set; }
    public bool IsAuthenticated { get; set; }
}