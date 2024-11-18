using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Localizations;
using FluentAssertions;
using Localizations.Domain;
using Localizations.Mediatr.Mediatr.Localizations.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Localizations;

[TestFixture]
public class LocalizationsRequestHandlerTest(): CommonIntegrationTestSetup()
{
    private static IEnumerable<TestCaseData> CreateLocalizationsTestCases()
    {
        // UserRole, IsPublic
        yield return new TestCaseData(UserRoleEnum.User, false).SetName("Non public localizations for User role");
        yield return new TestCaseData(UserRoleEnum.User, true).SetName("Public localizations for User role");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, false).SetName("Non public localizations for Technical Support role");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, true).SetName("Public localizations for Technical Support role");
        yield return new TestCaseData(UserRoleEnum.Admin, false).SetName("Non public localizations for Admin role");
        yield return new TestCaseData(UserRoleEnum.Admin, true).SetName("Public localizations for Admin role");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, false).SetName("Non public localizations for Super Admin role");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, true).SetName("Public localizations for Super Admin role");
    }
    
    [Test, TestCaseSource(nameof(CreateLocalizationsTestCases))]
    public async Task Handle_ShouldReturnLocalizations_WhenLocalizationRequestIsValid(UserRoleEnum role, bool isPublic)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity userToBeSignIn = await this.CreateTestUser(role);
        
        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            LocalizationsDataContext localizationsDataContext = scope.ServiceProvider.GetRequiredService<LocalizationsDataContext>();
            bool beforeSignIn = await this.IsCurrentUserAuthenticated();
            
            IMediator mediator = new Mediator(scope.ServiceProvider);
            LocalizationsResponse response = await mediator.Send(new LocalizationsRequest
            {
                IsPublic = isPublic
            });
            
            bool afterSignIn = await this.IsCurrentUserAuthenticated();
            
            // Assert
            beforeSignIn.Should().BeTrue();
            afterSignIn.Should().BeTrue();
            response.Should().NotBeNull();
            response.Version.Should().NotBeNullOrEmpty();
            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Should().HaveCount(await localizationsDataContext.Locales.CountAsync());
            int count = await localizationsDataContext.Localizations.CountAsync(x =>
                x.LocaleId == 1 && x.IsPublic == isPublic);
            response.Data.Should().OnlyContain(data => data.Items.Count == count);
        }
    }
    
    [Test]
    public async Task Handle_ShouldReturnPublicLocalizations_WhenLocalizationRequestIsValid()
    {
        // Arrange
        await this.SignOutUserIfExist();
        
        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            LocalizationsDataContext localizationsDataContext = scope.ServiceProvider.GetRequiredService<LocalizationsDataContext>();
            bool beforeSignIn = await this.IsCurrentUserAuthenticated();
            
            IMediator mediator = new Mediator(scope.ServiceProvider);
            LocalizationsResponse response = await mediator.Send(new LocalizationsRequest
            {
                IsPublic = true
            });
            
            bool afterSignIn = await this.IsCurrentUserAuthenticated();
            
            // Assert
            beforeSignIn.Should().BeFalse();
            afterSignIn.Should().BeFalse();
            response.Should().NotBeNull();
            response.Version.Should().NotBeNullOrEmpty();
            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Should().HaveCount(await localizationsDataContext.Locales.CountAsync());
            int count = await localizationsDataContext.Localizations.CountAsync(x =>
                x.LocaleId == 1 && x.IsPublic == true);
            response.Data.Should().OnlyContain(data => data.Items.Count == count);
        }
    }
}