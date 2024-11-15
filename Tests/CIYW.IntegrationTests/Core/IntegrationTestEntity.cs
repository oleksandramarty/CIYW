using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Extensions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CIYW.IntegrationTests.Core;

public class IntegrationTestEntity
{
    private string? _token { get; set; }
    private UserEntity? _user { get; set; }
    private UserProjectEntity? _userProject { get; set; }
    private UserRoleEnum? _role { get; set; }

    public string? Token
    {
        get => _token;
        set => _token = value;
    }

    public UserEntity? User
    {
        get => _user;
        set => _user = value;
    }

    public UserProjectEntity? UserProject
    {
        get => _userProject;
        set => _userProject = value;
    }

    public UserRoleEnum? Role
    {
        get => _role;
        set => _role = value;
    }

    public async Task InitializeUser(IntegrationTestBase testApplicationFactory)
    {
        if (this._role == null)
        {
            return;
        }

        await this.ReinitializeUser(testApplicationFactory);
    }

    public async Task Dispose(IntegrationTestBase testApplicationFactory)
    {
        using (var scope = testApplicationFactory.Services.CreateScope())
        {
            AuthGatewayDataContext authGatewayDataContext =
                scope.ServiceProvider.GetRequiredService<AuthGatewayDataContext>();
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();
            ITokenRepository tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();

            if (this._user != null)
            {
                // Remove the user project
                if (this._userProject != null)
                {
                    expensesDataContext.UserProjects.Remove(this._userProject);
                    await expensesDataContext.SaveChangesAsync();
                }

                // Remove the user
                authGatewayDataContext.Users.Remove(this._user);
                await authGatewayDataContext.SaveChangesAsync();

                // Remove the token
                await tokenRepository.RemoveTokenAsync(this._token);
            }
        }
    }

    private async Task ReinitializeUser(IntegrationTestBase testApplicationFactory)
    {
        using (var scope = testApplicationFactory.Services.CreateScope())
        {
            AuthGatewayDataContext authGatewayDataContext =
                scope.ServiceProvider.GetRequiredService<AuthGatewayDataContext>();
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();
            IJwtTokenFactory jwtTokenFactory = scope.ServiceProvider.GetRequiredService<IJwtTokenFactory>();
            ITokenRepository tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();

            string login = IntegrationTestConstants.UserName + StringExtension.GenerateRandomString(5);
            Guid userId = Guid.NewGuid();

            this._user = new UserEntity
            {
                Id = userId,
                Login = login,
                LoginNormalized = login.ToUpper(),
                Email = login + "@mail.com",
                EmailNormalized = (login + "@mail.com").ToUpper(),
                IsActive = true,
                IsTemporaryPassword = true,
                AuthType = UserAuthMethodEnum.Base,
                Roles = new List<UserRoleEntity>
                {
                    new UserRoleEntity
                    {
                        Id = Guid.NewGuid(),
                        UserId = userId,
                        RoleId = (int)this._role
                    }
                },
                UserSetting = new UserSettingEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    DefaultLocale = "en"
                }
            };

            this._user.Salt = jwtTokenFactory.GenerateSalt();
            this._user.PasswordHash = jwtTokenFactory.HashPassword(IntegrationTestConstants.UserName, this._user.Salt);

            await authGatewayDataContext.Users.AddAsync(this._user);
            await authGatewayDataContext.SaveChangesAsync();

            Guid userProjectId = Guid.NewGuid();

            this._userProject = new UserProjectEntity
            {
                Id = userProjectId,
                Title = $"{this._user.Login}'s project",
                IsActive = true,
                CreatedUserId = this._user.Id,
                Balances = new List<BalanceEntity>
                {
                    new BalanceEntity
                    {
                        Id = Guid.NewGuid(),
                        UserId = this._user.Id,
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

            await expensesDataContext.UserProjects.AddAsync(this._userProject);
            await expensesDataContext.SaveChangesAsync();

            this._user = await authGatewayDataContext.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .Include(u => u.UserSetting)
                .FirstOrDefaultAsync(u => u.Id == this._user.Id);

            var token = jwtTokenFactory.GenerateJwtToken(
                this._user.Id,
                this._user.Login,
                this._user.Email,
                string.Join(",", this._user.Roles.Select(r => r.Role.Title)),
                true);

            await tokenRepository.AddTokenAsync(token, TimeSpan.FromDays(30));

            this._token = $"{AuthSchema.Schema} {token}";
        }
    }
}