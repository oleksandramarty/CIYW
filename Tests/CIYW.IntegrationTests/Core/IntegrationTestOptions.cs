using System.Security.Claims;
using AuditTrail.Domain;
using AuditTrail.Domain.Models;
using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CIYW.IntegrationTests.Core;

/// <summary>
/// Options for integration tests
/// </summary>
public class IntegrationTestOptions
{
    private IntegrationTestUserEntity? CurrentUserEntity { get; set; }

    private UserRoleEnum? Role { get; set; }

    public IntegrationTestOptions()
    {
    }

    public IntegrationTestOptions(UserRoleEnum role)
    {
        Role = role;
    }

    public async Task InitializeUser(IntegrationTestBase testApplicationFactory)
    {
        if (!Role.HasValue)
        {
            return;
        }

        CurrentUserEntity = await this.CreateUser(testApplicationFactory, Role.Value);
    }

    public HttpContextAccessorForTesting GenerateClaims()
    {
        HttpContextAccessorForTesting httpContextAccessorForTesting = new HttpContextAccessorForTesting();

        if (this.CurrentUserEntity == null)
        {
            httpContextAccessorForTesting.HttpContext = new DefaultHttpContext()
            {
                User = null
            };
            return httpContextAccessorForTesting;
        }
        
        List<Claim> claims = this.TestClaims();
        
        ClaimsIdentity identity = new ClaimsIdentity(claims, "IntegrationTestAuthentication");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
        httpContextAccessorForTesting.HttpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };
        return httpContextAccessorForTesting;
    }

    public async Task<IntegrationTestUserEntity> CreateUser(IntegrationTestBase testApplicationFactory,
        UserRoleEnum role, bool withSignIn = true)
    {
        using var scope = testApplicationFactory.Services.CreateScope();
        AuthGatewayDataContext authGatewayDataContext =
            scope.ServiceProvider.GetRequiredService<AuthGatewayDataContext>();
        ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();
        IJwtTokenFactory jwtTokenFactory = scope.ServiceProvider.GetRequiredService<IJwtTokenFactory>();
        ITokenRepository tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();

        string login = IntegrationTestConstants.UserName + StringExtension.GenerateRandomString(5);
        Guid userId = Guid.NewGuid();
        string salt = jwtTokenFactory.GenerateSalt();
        string passwordHash = jwtTokenFactory.HashPassword(IntegrationTestConstants.UserName, salt);
            
        UserEntity user = new UserEntity
        {
            Id = userId,
            Login = login,
            LoginNormalized = login.ToUpper(),
            Email = login + "@mail.com",
            EmailNormalized = (login + "@mail.com").ToUpper(),
            IsActive = true,
            IsTemporaryPassword = true,
            AuthType = UserAuthMethodEnum.Base,
            Salt = salt,
            PasswordHash = passwordHash,
            Roles = new List<UserRoleEntity>
            {
                new UserRoleEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    RoleId = (int)role
                }
            },
            UserSetting = new UserSettingEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DefaultLocale = "en"
            }
        };

        await authGatewayDataContext.Users.AddAsync(user);
        await authGatewayDataContext.SaveChangesAsync();

        Guid userProjectId = Guid.NewGuid();

        UserProjectEntity userProject = new UserProjectEntity
        {
            Id = userProjectId,
            Title = $"{user.Login}'s project",
            IsActive = true,
            CreatedUserId = user.Id,
            Balances = new List<BalanceEntity>
            {
                new BalanceEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Amount = 0.0m,
                    CurrencyId = IntegrationTestConstants.DefaultCurrencyId,
                    Title = "Balance",
                    IconId = IntegrationTestConstants.DefaultIconId,
                    UserProjectId = userProjectId,
                    BalanceTypeId = IntegrationTestConstants.DefaultBalanceTypeId,
                    IsActive = true
                }
            }
        };

        await expensesDataContext.UserProjects.AddAsync(userProject);
        await expensesDataContext.SaveChangesAsync();
        
        IntegrationTestUserEntity result = new IntegrationTestUserEntity
        {
            Role = role,
            User = await authGatewayDataContext.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .Include(u => u.UserSetting)
                .FirstOrDefaultAsync(u => u.Id == user.Id),
            UserProjects = new List<UserProjectEntity> { userProject }
        };

        if (withSignIn)
        {
            if (result.User == null)
            {
                throw new ArgumentNullException(nameof(result.User));
            }
                
            CurrentUserEntity = result;
            Role = role;
                
            var token = jwtTokenFactory.GenerateJwtToken(
                CurrentUserEntity.User.Id,
                CurrentUserEntity.User.Login,
                CurrentUserEntity.User.Email,
                string.Join(",", Role.Value.ToString()),
                true);

            await tokenRepository.AddTokenAsync(token, TimeSpan.FromDays(30));


            var httpContextAccessorForTesting = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            List<Claim> claims = this.TestClaims();

            ClaimsIdentity identity = new ClaimsIdentity(claims, "IntegrationTestAuthentication");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            httpContextAccessorForTesting.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
        }

        return result;
    }

    private List<Claim> TestClaims()
    {
        if (this.CurrentUserEntity?.User == null)
        {
            throw new ArgumentNullException(nameof(this.CurrentUserEntity.User));
        }

        if (this.CurrentUserEntity.User.Roles == null || !this.CurrentUserEntity.User.Roles.Any())
        {
            throw new ArgumentNullException(nameof(this.CurrentUserEntity.User.Roles));
        }

        bool rememberMe = true;

        List<Claim> claims =
        [
            new Claim(AuthClaims.Login, this.CurrentUserEntity.User.Login),
            new Claim(AuthClaims.Email, this.CurrentUserEntity.User.Email),
            new Claim(AuthClaims.UserId, this.CurrentUserEntity.User.Id.ToString()),
            new Claim(AuthClaims.Role,
                this.CurrentUserEntity.User.Roles.FirstOrDefault()?.Role?.UserRole.ToString() ?? string.Empty),
            new Claim(AuthClaims.RememberMe, rememberMe.ToString())
        ];

        return claims;
    }

    public async Task Dispose(IntegrationTestBase testApplicationFactory)
    {
        using var scope = testApplicationFactory.Services.CreateScope();
        AuthGatewayDataContext authGatewayDataContext =
            scope.ServiceProvider.GetRequiredService<AuthGatewayDataContext>();
        ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();
        AuditTrailDataContext auditTrailDataContext =
            scope.ServiceProvider.GetRequiredService<AuditTrailDataContext>();

        await this.RemoveAllFromTable<AuthGatewayDataContext, UserSettingEntity>(authGatewayDataContext);
        await this.RemoveAllFromTable<AuthGatewayDataContext, UserRoleEntity>(authGatewayDataContext);
        await this.RemoveAllFromTable<AuthGatewayDataContext, UserEntity>(authGatewayDataContext);

        await this.RemoveAllFromTable<ExpensesDataContext, ExpenseEntity>(expensesDataContext);
        await this.RemoveAllFromTable<ExpensesDataContext, PlannedExpenseEntity>(expensesDataContext);
        await this.RemoveAllFromTable<ExpensesDataContext, FavoriteExpenseEntity>(expensesDataContext);
        await this.RemoveAllFromTable<ExpensesDataContext, BalanceEntity>(expensesDataContext);
        await this.RemoveAllFromTable<ExpensesDataContext, UserAllowedProjectEntity>(expensesDataContext);
        await this.RemoveAllFromTable<ExpensesDataContext, UserProjectEntity>(expensesDataContext);

        await this.RemoveAllFromTable<AuditTrailDataContext, AuditTrailEntity>(auditTrailDataContext);
        await this.RemoveAllFromTable<AuditTrailDataContext, AuditTrailArchiveEntity>(auditTrailDataContext);
    }

    private async Task RemoveAllFromTable<TDataContext, TEntity>(TDataContext context)
        where TDataContext : DbContext
        where TEntity : class
    {
        List<TEntity> entities = await context.Set<TEntity>().ToListAsync();

        context.RemoveRange(entities);
        await context.SaveChangesAsync();
    }
}