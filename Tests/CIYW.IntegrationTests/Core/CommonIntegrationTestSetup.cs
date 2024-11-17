using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Enums;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Core;

/// <summary>
/// Common setup for integration tests
/// </summary>
public class CommonIntegrationTestSetup: IDisposable 
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
    public CommonIntegrationTestSetup(UserRoleEnum? role)
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
    /// <returns></returns>
    public async Task<IntegrationTestUserEntity> CreateTestUser(UserRoleEnum role, bool withSignIn = true)
    {
        return await Options.CreateUser(TestApplicationFactory, role, withSignIn);
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
}