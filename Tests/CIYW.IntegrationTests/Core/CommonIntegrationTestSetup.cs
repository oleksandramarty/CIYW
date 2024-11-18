using AuthGateway.Domain.Models.Users;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Common.BaseInterfaces;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using Dictionaries.Domain;
using Dictionaries.Mediatr.Mediatr.Requests;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Core;

/// <summary>
/// Common setup for integration tests
/// </summary>
public class CommonIntegrationTestSetup : IDisposable
{
    /// <summary>
    /// Http client for integration tests
    /// </summary>
    protected HttpClient Client { get; set; }

    /// <summary>
    /// Test application factory
    /// </summary>
    protected IntegrationTestBase TestApplicationFactory;

    /// <summary>
    /// Options for integration tests
    /// </summary>
    protected IntegrationTestOptions Options { get; }

    /// <summary>
    /// Start with specific user or without user
    /// </summary>
    /// <param name="role">User role</param>
    public CommonIntegrationTestSetup(UserRoleEnum? role = null)
    {
        Options = role.HasValue ? new IntegrationTestOptions(role.Value) : new IntegrationTestOptions();
    }

    /// <summary>
    /// Setup for integration tests
    /// Create factory and client
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        TestApplicationFactory = new IntegrationTestBase(Options);
        this.Client = TestApplicationFactory.CreateClient();

        await Options.InitializeUser(TestApplicationFactory);
    }

    /// <summary>
    /// Create test user
    /// </summary>
    /// <param name="role">User role</param>
    /// <param name="withSignIn">Sign in user flag</param>
    /// <param name="userActions">User actions</param>
    /// <returns></returns>
    public async Task<IntegrationTestUserEntity> CreateTestUser(
        UserRoleEnum role,
        IEnumerable<Action<UserEntity>>? userActions = null,
        bool withSignIn = true)
    {
        return await Options.CreateUser(TestApplicationFactory, role, withSignIn, userActions);
    }

    /// <summary>
    /// Sign out user if exist
    /// </summary>
    public async Task SignOutUserIfExist()
    {
        await Options.SignOutUserIfExist(TestApplicationFactory);
    }

    /// <summary>
    /// Check if current user is authenticated
    /// </summary>
    /// <returns></returns>
    public async Task<bool> IsCurrentUserAuthenticated()
    {
        return await this.Options.IsCurrentUserAuthenticated(TestApplicationFactory);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Options.Dispose(TestApplicationFactory);
        this.Dispose();
    }

    /// <summary>
    /// Dispose client
    /// </summary>
    public void Dispose()
    {
        Client.Dispose();
    }

    public static IEnumerable<TestCaseData> CreateAllRolesTestCases()
    {
        yield return new TestCaseData(UserRoleEnum.User).SetName("User").SetDescription("User role");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport).SetName("TechnicalSupport")
            .SetDescription("TechnicalSupport role");
        yield return new TestCaseData(UserRoleEnum.Admin).SetName("Admin").SetDescription("Admin role");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin).SetName("SuperAdmin").SetDescription("SuperAdmin role");
    }

    public async Task<SiteSettingsResponse> SiteSettings()
    {
        return await this.Options.SiteSettings(TestApplicationFactory);
    }

    public async Task HandleValidDictionary<TRequest, TEntity, TEntityResponse>(
        UserRoleEnum role, 
        string? version,
        int count = 0)
        where TRequest : IRequest<VersionedListResponse<TEntityResponse>>, IBaseVersionEntity, new ()
        where TEntityResponse : class
        where TEntity : class
    {
        // Arrange
        await this.SignOutUserIfExist();
        await this.CreateTestUser(role);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            DictionariesDataContext dictionariesDataContext =
                scope.ServiceProvider.GetRequiredService<DictionariesDataContext>();

            IMediator mediator = new Mediator(scope.ServiceProvider);
            
            TRequest request = new TRequest
            {
                Version = version
            };
            
            VersionedListResponse<TEntityResponse> response = await mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            if (count != 0)
            {
                response.Items.Should().NotBeNullOrEmpty();
            }
            
            response.Items.Should().HaveCount(count);
        }
    }
}