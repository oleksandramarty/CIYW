using CommonModule.Shared.Common;

namespace CommonModule.GraphQL;

public static class GraphQLEndpoints
{
    public static readonly GraphQLEndpoint CurrentUser = new GraphQLEndpoint("auth_gateway_current_user");
    public static readonly GraphQLEndpoint SiteSettings = new GraphQLEndpoint("dictionaries_site_settings", false);
    public static readonly GraphQLEndpoint SignIn = new GraphQLEndpoint("auth_gateway_sign_in", false);
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