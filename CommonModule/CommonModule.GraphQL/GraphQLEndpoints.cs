using CommonModule.Shared.Common;

namespace CommonModule.GraphQL;

public static class GraphQLEndpoints
{
    public static readonly GraphQLEndpoint CurrentUser = new GraphQLEndpoint("auth_gateway_current_user");
    public static readonly GraphQLEndpoint SiteSettings = new GraphQLEndpoint("dictionaries_site_settings", false);
    public static readonly GraphQLEndpoint SignIn = new GraphQLEndpoint("auth_gateway_sign_in", false);
    public static readonly GraphQLEndpoint CreateExpense = new GraphQLEndpoint("expenses_create_expense");
    public static readonly GraphQLEndpoint UpdateExpense = new GraphQLEndpoint("expenses_update_expense");
    public static readonly GraphQLEndpoint RemoveExpense = new GraphQLEndpoint("expenses_remove_expense");
    public static readonly GraphQLEndpoint CreatePlannedExpense = new GraphQLEndpoint("expenses_create_planned_expense");
    public static readonly GraphQLEndpoint UpdatePlannedExpense = new GraphQLEndpoint("expenses_update_planned_expense");
    public static readonly GraphQLEndpoint RemovePlannedExpense = new GraphQLEndpoint("expenses_remove_planned_expense");
    public static readonly GraphQLEndpoint CreateUserProject = new GraphQLEndpoint("expenses_create_user_project");
    public static readonly GraphQLEndpoint GetFilteredExpenses = new GraphQLEndpoint("expenses_get_filtered_expenses");
    public static readonly GraphQLEndpoint GetFilteredPlannedExpenses = new GraphQLEndpoint("expenses_get_filtered_planned_expenses");
    public static readonly GraphQLEndpoint GetUserProjectById = new GraphQLEndpoint("expenses_get_user_project_by_id");
    public static readonly GraphQLEndpoint GetUserProjects = new GraphQLEndpoint("expenses_get_user_projects");
    public static readonly GraphQLEndpoint GetUserAllowedProjects = new GraphQLEndpoint("expenses_get_user_allowed_projects");
}


public class GraphQLEndpoint
{
    public GraphQLEndpoint(string name, bool isAuthenticated = true)
    {
        Name = name;
        IsAuthenticated = isAuthenticated;
    }
    
    public string Name { get; set; }
    public bool IsAuthenticated { get; set; }
}