using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
using FluentAssertions;
using Localizations.Domain;
using Localizations.Mediatr.Mediatr.Localizations.Requests;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Localizations;

[TestFixture]
public class LocalesRequestHandlerTest(): CommonIntegrationTestSetup()
{
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnLocales_WhenLocalesRequestIsValid(UserRoleEnum role)
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
            VersionedListResponse<LocaleResponse> response = await mediator.Send(new LocalesRequest());
            
            bool afterSignIn = await this.IsCurrentUserAuthenticated();
            
            // Assert
            beforeSignIn.Should().BeTrue();
            afterSignIn.Should().BeTrue();
            response.Should().NotBeNull();
            response.Version.Should().NotBeNullOrEmpty();
            response.Items.Should().NotBeNullOrEmpty();
            response.Items.Should().HaveCount(await localizationsDataContext.Locales.CountAsync());
        }
    }
}