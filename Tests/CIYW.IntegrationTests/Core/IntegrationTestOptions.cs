using System.Security.Claims;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace CIYW.IntegrationTests.Core;

/// <summary>
/// Options for integration tests
/// </summary>
public class IntegrationTestOptions: IntegrationTestEntity
{
    public IntegrationTestOptions()
    {
        
    }

    public IntegrationTestOptions(UserRoleEnum role)
    {
        this.Role = role;
    }

    /// <summary>
    /// Generate claims for test uer
    /// </summary>
    /// <returns></returns>
    public HttpContextAccessorForTesting GenerateClaims()
    {
        HttpContextAccessorForTesting httpContextAccessorForTesting = new HttpContextAccessorForTesting();
        
        if (this.User == null)
        {
            httpContextAccessorForTesting.HttpContext = new DefaultHttpContext
            {
                User = null
            };
            return httpContextAccessorForTesting;
        }
        
        List<Claim> claims = this.GetTestClaims();
        
        ClaimsIdentity? identity = new ClaimsIdentity(claims, "IntegrationTestAuthentication");
        ClaimsPrincipal? claimsPrincipal = new ClaimsPrincipal(identity);
        httpContextAccessorForTesting.HttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        return httpContextAccessorForTesting;
    }
    
    /// <summary>
    /// Get test claims by user id
    /// </summary>
    /// <returns></returns>
    private List<Claim> GetTestClaims()
    {
        if (this.User == null)
        {
            throw new ArgumentNullException(nameof(this.User));
        }
        
        if (this.User.Roles == null || !this.User.Roles.Any())
        {
            throw new ArgumentNullException(nameof(this.User.Roles));
        }
        
        bool rememberMe = true;
        
        List<Claim> claims = new List<Claim>
        {
            new Claim(AuthClaims.Login, this.User.Login),
            new Claim(AuthClaims.Email, this.User.Email),
            new Claim(AuthClaims.UserId, this.User.Id.ToString()),
            new Claim(AuthClaims.Role, this.User.Roles.FirstOrDefault().Role.UserRole.ToString()),
            new Claim(AuthClaims.RememberMe, rememberMe.ToString())
        };

        return claims;
    }
}