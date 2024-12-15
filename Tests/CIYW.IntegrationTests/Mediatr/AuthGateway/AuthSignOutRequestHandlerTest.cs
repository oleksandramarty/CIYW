using AuthGateway.Domain;
using AuthGateway.Domain.Models.Users;
using AuthGateway.Mediatr.Mediatr.Auth.Handlers;
using AuthGateway.Mediatr.Mediatr.Auth.Requests;
using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Exceptions;
using CommonModule.Interfaces;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Auth;
using CommonModule.Shared.Responses.Base;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.AuthGateway;

[TestFixture]
public class AuthSignOutRequestHandlerTest() : CommonIntegrationTestSetup()
{
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnTrue_WhenAuthSignOutRequestIsValid(UserRoleEnum role)
    {
        // Arrange
        await SignOutUserIfExist();
        await CreateTestUser(role);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            bool beforeSignIn = await IsCurrentUserAuthenticated();

            IMediator mediator = new Mediator(scope.ServiceProvider);
            BaseBoolResponse response = await mediator.Send(new AuthSignOutRequest());

            bool afterSignIn = await IsCurrentUserAuthenticated();

            // Assert
            beforeSignIn.Should().BeTrue();
            afterSignIn.Should().BeFalse();
            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesWithInvalidRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenAuthSignOutRequestWithBlockedUser(UserRoleEnum role, StatusEnum status, string errorMessage)
    {
        // Arrange
        await SignOutUserIfExist();
        IntegrationTestUserEntity userToBeSignIn = await CreateTestUser(
            role,
            1,
            1,
            true,
            [
                user => user.Status = status
            ]
        );

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            bool beforeSignIn = await IsCurrentUserAuthenticated();

            beforeSignIn.Should().BeTrue();
            IMediator mediator = new Mediator(scope.ServiceProvider);

            await TestUtilities.Handle_InvalidCommand<AuthSignInRequest, JwtTokenResponse, BusinessException>(
                mediator,
                new AuthSignInRequest
                {
                    Login = userToBeSignIn.User.Login,
                    Password = userToBeSignIn.User.Login,
                    RememberMe = true
                },
                errorMessage);
        }
    }
}