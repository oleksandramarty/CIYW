using CommonModule.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommonModule.Core.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class UserInRoleAttribute : Attribute, IAuthorizationFilter
{
    private readonly UserRoleEnum[] _roles;

    public UserInRoleAttribute(params UserRoleEnum[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user == null || !_roles.Any(role => user.IsInRole(role.ToString())))
        {
            context.Result = new ForbidResult();
        }
    }
}