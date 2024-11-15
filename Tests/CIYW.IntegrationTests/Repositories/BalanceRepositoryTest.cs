using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Repositories;

[TestFixture]
public class BalanceRepositoryTest() : CommonIntegrationTestSetup(UserRoleEnum.User)
{
    [Test]
    public async Task TestApplicationRuns()
    {
        // Arrange
        var isAppRunning = true;

        // Act
        // Here you would typically have code to start the application or check its status


        using (var scope = this.testApplicationFactory.Services.CreateScope())
        {
            var httpContextAccessorForTesting = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
            var t1 = httpContextAccessorForTesting.HttpContext.User;
            bool isAuthenticated = t1?.Identity?.IsAuthenticated ?? false;
            isAuthenticated.Should().BeTrue();

            IntegrationTestUserEntity user1 = await this.CreateTestUser(UserRoleEnum.User, false);
            var t2 = httpContextAccessorForTesting.HttpContext.User;
            IntegrationTestUserEntity user2 = await this.CreateTestUser(UserRoleEnum.User);
            var t3 = httpContextAccessorForTesting.HttpContext.User;
        }

        // Assert
        isAppRunning.Should().BeTrue();
    }
}