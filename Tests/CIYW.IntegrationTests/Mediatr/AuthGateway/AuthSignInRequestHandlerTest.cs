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
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.AuthGateway;

[TestFixture]
public class AuthSignInRequestHandlerTest() : CommonIntegrationTestSetup()
{
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnTokenResponse_WhenAuthSignInRequestIsValid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity userToBeSignIn = await this.CreateTestUser(role, 1, 1, false);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            IJwtTokenFactory jwtTokenFactory = scope.ServiceProvider.GetRequiredService<IJwtTokenFactory>();
            bool beforeSignIn = await this.IsCurrentUserAuthenticated();

            IMediator mediator = new Mediator(scope.ServiceProvider);
            JwtTokenResponse response = await mediator.Send(new AuthSignInRequest
            {
                Login = userToBeSignIn.User.Login,
                Password = userToBeSignIn.User.Login,
                RememberMe = true
            });
            userToBeSignIn.Token = response.Token;
            this.Options.CurrentUserEntity = userToBeSignIn;

            bool afterSignIn = await this.IsCurrentUserAuthenticated();
            Guid? userId = !string.IsNullOrEmpty(response?.Token)
                ? jwtTokenFactory.UserIdFromToken(response.Token)
                : null;

            // Assert
            beforeSignIn.Should().BeFalse();
            afterSignIn.Should().BeTrue();
            response.Should().NotBeNull();
            response.Token.Should().NotBeNullOrEmpty();
            userId.Should().NotBeNull();
            userId.Should().Be(userToBeSignIn.User.Id);
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenAuthSignInRequestIsInvalidPassword(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity userToBeSignIn = await this.CreateTestUser(role, 1, 1, false);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            bool beforeSignIn = await this.IsCurrentUserAuthenticated();

            beforeSignIn.Should().BeFalse();
            IMediator mediator = new Mediator(scope.ServiceProvider);

            await TestUtilities.Handle_InvalidCommand<AuthSignInRequest, JwtTokenResponse, AuthException>(
                mediator,
                new AuthSignInRequest
                {
                    Login = userToBeSignIn.User.Login,
                    Password = userToBeSignIn.User.Login + "1",
                    RememberMe = true
                },
                string.Format(ErrorMessages.WrongAuth, nameof(UserEntity)));
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenAuthSignInRequestIsInvalidLogin(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity userToBeSignIn = await this.CreateTestUser(role, 1, 1, false);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            bool beforeSignIn = await this.IsCurrentUserAuthenticated();

            beforeSignIn.Should().BeFalse();
            IMediator mediator = new Mediator(scope.ServiceProvider);

            await TestUtilities.Handle_InvalidCommand<AuthSignInRequest, JwtTokenResponse, EntityNotFoundException>(
                mediator,
                new AuthSignInRequest
                {
                    Login = userToBeSignIn.User.Login + "1",
                    Password = userToBeSignIn.User.Login,
                    RememberMe = true
                },
                string.Format(ErrorMessages.EntityNotFound, nameof(UserEntity)));
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesWithInvalidRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenAuthSignInRequestWithBlockedUser(UserRoleEnum role, StatusEnum status, string errorMessage)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity userToBeSignIn = await this.CreateTestUser(role, 1, 1, false,
            [
                user => user.Status = status
            ]
        );

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            bool beforeSignIn = await this.IsCurrentUserAuthenticated();

            beforeSignIn.Should().BeFalse();
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