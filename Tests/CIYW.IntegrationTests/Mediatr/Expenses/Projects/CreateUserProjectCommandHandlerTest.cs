using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Commands;
using Expenses.Mediatr.Validators.Projects;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Expenses.Projects;

[TestFixture]
public class CreateUserProjectCommandHandlerTest() : CommonIntegrationTestSetup()
{
    private static IEnumerable<TestCaseData> CreateAllRolesUserProjectTestCases()
    {
        foreach (UserRoleEnum role in Enum.GetValues(typeof(UserRoleEnum)))
        {
            yield return new TestCaseData(role, 0).SetName($"{role} role with 0 user projects");
            yield return new TestCaseData(role, 1).SetName($"{role} role with 1 user project");
            yield return new TestCaseData(role, 2).SetName($"{role} role with 2 user projects");
        }
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesUserProjectTestCases))]
    public async Task Handle_ShouldReturnBalanceId_WhenCreateUserProjectCommandIsValid(
        UserRoleEnum role,
        int userProjectsCount
        )
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role, userProjectsCount);
        
        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();
            
            IMediator mediator = new Mediator(scope.ServiceProvider);
            BaseEntityIdResponse<Guid> response = await mediator.Send(new CreateUserProjectCommand
            {
                Title = "Test"
            });
            
            // Assert
            List<UserProjectEntity> userProjects = await expensesDataContext.UserProjects.Where(up => up.CreatedUserId == Options.CurrentUserEntity.User.Id).ToListAsync();
            UserProjectEntity? createdUserProject = userProjects.FirstOrDefault(up => up.Id == response.Id);
            
            response.Should().NotBeNull();
            response.Id.Should().NotBeEmpty();
            createdUserProject.Should().NotBeNull();
            createdUserProject!.Title.Should().Be("Test");
            createdUserProject!.Status.Should().Be(StatusEnum.Active);
            userProjects.Should().NotBeNull();
            userProjects.Should().NotBeEmpty();
            userProjects.Count().Should().Be(userProjectsCount + 1);
        }
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenCreateUserProjectCommandIsInvalid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role, 3);
        
        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();
            
            IMediator mediator = new Mediator(scope.ServiceProvider);
            await TestUtilities.Handle_InvalidCommand<CreateUserProjectCommand, BaseEntityIdResponse<Guid>, BusinessException>(
                mediator,
                new CreateUserProjectCommand
                {
                    Title = "Test"
                },
                ErrorMessages.UserProjectLimitExceeded);
        }
    }
    
    private static IEnumerable<TestCaseData> CreateInvalidValidatorForSignUp()
    {
        // Title
        yield return new TestCaseData(null).SetName("Title is null");
        yield return new TestCaseData(string.Empty).SetName("Title is empty");
        yield return new TestCaseData(StringExtension.GenerateRandomString(101)).SetName("Title is 101 characters long");
    }

    [Test, TestCaseSource(nameof(CreateInvalidValidatorForSignUp))]
    public void Validator_ShouldHaveErrors_WhenSignUpRequestIsInvalid(string title)
    {
        // Arrange
        var validator = new CreateUserProjectCommandValidator();
        var invalidCommand = new CreateUserProjectCommand
        {
            Title = title
        };

        // Act
        ValidationResult result = validator.Validate(invalidCommand);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}