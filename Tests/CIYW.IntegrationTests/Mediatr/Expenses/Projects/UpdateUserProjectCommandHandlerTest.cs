using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Enums;
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
public class UpdateUserProjectCommandHandlerTest() : CommonIntegrationTestSetup()
{
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnBalanceId_WhenUpdateUserProjectCommandIsValid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role);
        Guid userProjectId = user.UserProjects.First().Id;
        
        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();

            string newTitle = StringExtension.GenerateRandomString(10);
            
            IMediator mediator = new Mediator(scope.ServiceProvider);
            await mediator.Send(new UpdateUserProjectCommand
            {
                Id = userProjectId, 
                Title = newTitle
            });
            
            // Assert
            List<UserProjectEntity> userProjects = await expensesDataContext.UserProjects.Where(up => up.CreatedUserId == Options.CurrentUserEntity.User.Id).ToListAsync();
            UserProjectEntity? updatedUserProject = userProjects.FirstOrDefault(up => up.Id == userProjectId);
            
            userProjects.Should().NotBeNull();
            userProjects.Should().NotBeEmpty();
            updatedUserProject.Should().NotBeNull();
            updatedUserProject.Title.Should().Be(newTitle);
            updatedUserProject.Status.Should().Be(StatusEnum.Active);
        }
    }
    
    private static IEnumerable<TestCaseData> UpdateInvalidValidatorForSignUp()
    {
        // Title
        yield return new TestCaseData(null).SetName("Title is null");
        yield return new TestCaseData(string.Empty).SetName("Title is empty");
        yield return new TestCaseData(StringExtension.GenerateRandomString(101)).SetName("Title is 101 characters long");
    }

    [Test, TestCaseSource(nameof(UpdateInvalidValidatorForSignUp))]
    public void Validator_ShouldHaveErrors_WhenSignUpRequestIsInvalid(string title)
    {
        // Arrange
        var validator = new UpdateUserProjectCommandValidator();
        var invalidCommand = new UpdateUserProjectCommand
        {
            Title = title
        };

        // Act
        ValidationResult result = validator.Validate(invalidCommand);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}