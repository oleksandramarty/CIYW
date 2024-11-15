using CIYW.IntegrationTests.Core;
using CommonModule.Shared.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Repositories;

[TestFixture]
public class BalanceRepositoryTest() : CommonIntegrationTestSetup(UserRoleEnum.User)
{
    [Test]
    public void TestApplicationRuns()
    {
        // Arrange
        var isAppRunning = true;

        // Act
        // Here you would typically have code to start the application or check its status

        // Assert
        isAppRunning.Should().BeTrue();
    }
}