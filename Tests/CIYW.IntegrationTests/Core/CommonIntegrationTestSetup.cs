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
    protected IntegrationTestBase testApplicationFactory;
    
    /// <summary>
    /// Options for integration tests
    /// </summary>
    protected IntegrationTestOptions options { get;}
    
    /// <summary>
    /// Default start without user
    /// </summary>
    public CommonIntegrationTestSetup()
    {
        this.options = new IntegrationTestOptions();
    }

    /// <summary>
    /// Start with specific user or without user
    /// </summary>
    /// <param name="role">User role</param>
    public CommonIntegrationTestSetup(UserRoleEnum role)
    {
        this.options = new IntegrationTestOptions(role);
    }

    /// <summary>
    /// Setup for integration tests
    /// Create factory and client
    /// </summary>
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        this.testApplicationFactory = new IntegrationTestBase(this.options);
        this.Client = this.testApplicationFactory.CreateClient();
        
        await this.options.InitializeUser(this.testApplicationFactory);
    }
    
    /// <summary>
    /// Create test user
    /// </summary>
    /// <param name="role">User role</param>
    /// <param name="withSignIn">Sign in user flag</param>
    /// <returns></returns>
    public async Task<IntegrationTestUserEntity> CreateTestUser(UserRoleEnum role, bool withSignIn = true)
    {
        return await this.options.CreateUser(this.testApplicationFactory, role, withSignIn);
    }
    
    /// <summary>
    /// Dispose
    /// </summary>
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await this.options.Dispose(this.testApplicationFactory);
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