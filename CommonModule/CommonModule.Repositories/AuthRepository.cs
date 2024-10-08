using System.Security.Claims;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using Microsoft.AspNetCore.Http;

namespace CommonModule.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public AuthRepository(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentToken()
    {
        var authorizationHeader = this.httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
        return authorizationHeader.StartsWith($"{AuthSchema.Schema} ")
            ? authorizationHeader.Substring($"{AuthSchema.Schema} ".Length).Trim()
            : null;
    }

    public IEnumerable<Claim> GetCurrentClaims()
    {
        return this.httpContextAccessor.HttpContext.User.Claims;
    }

    public Guid? GetCurrentUserId()
    {
        var userIdClaim = this.httpContextAccessor.HttpContext?.User.FindFirst(AuthClaims.UserId);
        if (userIdClaim == null)
        {
            return null;
        }

        if (Guid.TryParse(userIdClaim.Value, out var userId))
        {
            return userId;
        }

        return null;
    }

    public UserRoleEnum GetCurrentUserRole()
    {
        var roleString = this.httpContextAccessor.HttpContext.User.FindFirst(AuthClaims.Role)?.Value;
        if (Enum.TryParse<UserRoleEnum>(roleString, out var role))
        {
            return role;
        }

        throw new EntityNotFoundException();
    }

    public Task<string> GetCurrentTokenAsync()
    {
        var token = GetCurrentToken();
        return Task.FromResult(token);
    }

    public Task<IEnumerable<Claim>> GetCurrentClaimsAsync()
    {
        var claims = GetCurrentClaims();
        return Task.FromResult(claims);
    }

    public Task<Guid?> GetCurrentUserIdAsync()
    {
        return Task.FromResult(GetCurrentUserId());
    }

    public Task<UserRoleEnum> GetCurrentUserRoleAsync()
    {
        return Task.FromResult(GetCurrentUserRole());
    }

    public bool IsAuthenticated()
    {
        var currentUserId = GetCurrentUserId();
        return currentUserId != null;
    }

    public async Task<bool> HasUserInRoleAsync(UserRoleEnum role)
    {
        UserRoleEnum currentUserRole = await GetCurrentUserRoleAsync();
        return currentUserRole == role;
    }
}