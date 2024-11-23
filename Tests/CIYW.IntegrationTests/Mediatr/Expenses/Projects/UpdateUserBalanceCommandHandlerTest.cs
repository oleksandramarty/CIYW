using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Enums;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
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
public class UpdateUserBalanceCommandHandlerTest() : CommonIntegrationTestSetup()
{
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnBalanceId_WhenUpdateUserBalanceCommandIsValid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role);
        
        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();

            string newTitle = StringExtension.GenerateRandomString(10);
            
            IMediator mediator = new Mediator(scope.ServiceProvider);
            await mediator.Send(new UpdateUserBalanceCommand
            {
                Id = user.UserProjects.First().Balances.First().Id,
                UserProjectId = user.UserProjects.First().Id,
                CurrencyId = IntegrationTestConstants.DefaultCurrencyId,
                IconId = IntegrationTestConstants.DefaultIconId,
                Title = newTitle,
                IsActive = true,
                BalanceTypeId = 1
            });
            
            // Assert
            List<BalanceEntity> userBalances = await expensesDataContext.Balances.Where(up => up.UserId == Options.CurrentUserEntity.User.Id).ToListAsync();
            
            userBalances.Should().NotBeNull();
            userBalances.Should().NotBeEmpty();
            userBalances.Count(up => up.Title == newTitle).Should().Be(1);
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
        var validator = new UpdateUserBalanceCommandValidator();
        var invalidCommand = new UpdateUserBalanceCommand
        {
            Title = title,
            IsActive = true
        };

        // Act
        ValidationResult result = validator.Validate(invalidCommand);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}