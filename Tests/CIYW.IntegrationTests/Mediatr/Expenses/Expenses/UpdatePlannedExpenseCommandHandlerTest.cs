using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Enums;
using Expenses.Domain;
using Expenses.Domain.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using Expenses.Mediatr.Validators.Expenses;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Expenses.Expenses;

[TestFixture]
public class UpdatePlannedExpenseCommandHandlerTest : CommonIntegrationTestSetup
{
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldUpdatePlannedExpense_WhenUpdatePlannedExpenseCommandIsValid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role);
        UserProjectEntity userProject = user.UserProjects.First();
        Guid balanceId = userProject.Balances.First().Id;
        var plannedExpenses = new Dictionary<int, int>
        {
            { 1, 1 }
        };
        await this.AddAllExpenses(
            user.User.Id,
            userProject.Id,
            balanceId,
            null,
            plannedExpenses
        );

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();
            
            PlannedExpenseEntity existingExpense = await expensesDataContext
                .PlannedExpenses
                .FirstOrDefaultAsync(fe =>
                    fe.CreatedUserId == Options.CurrentUserEntity.User.Id &&
                    fe.UserProjectId == userProject.Id
                );
            
            IMediator mediator = new Mediator(scope.ServiceProvider);
            await mediator.Send(new UpdatePlannedExpenseCommand
            {
                Id = existingExpense.Id,
                Title = "Updated Test",
                Description = "Updated Description",
                Amount = 200,
                CategoryId = 2,
                BalanceId = balanceId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                FrequencyId = 2,
                Status = StatusEnum.Active
            });

            // Assert
            PlannedExpenseEntity? updatedExpense = await expensesDataContext
                .PlannedExpenses
                .FirstOrDefaultAsync(fe =>
                    fe.CreatedUserId == Options.CurrentUserEntity.User.Id &&
                    fe.UserProjectId == userProject.Id &&
                    fe.Id == existingExpense.Id
                );

            updatedExpense.Should().NotBeNull();
            updatedExpense!.Title.Should().Be("Updated Test");
            updatedExpense!.Description.Should().Be("Updated Description");
            updatedExpense!.Amount.Should().Be(200);
            updatedExpense!.CategoryId.Should().Be(2);
            updatedExpense!.BalanceId.Should().Be(balanceId);
            updatedExpense!.EndDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(1), TimeSpan.FromSeconds(1));
            updatedExpense!.UserProjectId.Should().Be(userProject.Id);
            updatedExpense!.FrequencyId.Should().Be(2);
        }
    }

    private static IEnumerable<TestCaseData> CreateInvalidValidatorForUpdateExpense()
    {
        // Title
        yield return new TestCaseData(null, "Description", 10.0m, DateTime.UtcNow, null).SetName("Title is null");
        yield return new TestCaseData(string.Empty, "Description", 10.0m, DateTime.UtcNow, null).SetName(
            "Title is empty");
        yield return new TestCaseData(new string('a', 51), "Description", 10.0m, DateTime.UtcNow, null).SetName(
            "Title is 51 characters long");

        // Description
        yield return new TestCaseData("Title", new string('a', 101), 10.0m, DateTime.UtcNow, null).SetName(
            "Description is 101 characters long");

        // Amount
        yield return new TestCaseData("Title", "Description", 0.0m, DateTime.UtcNow, null).SetName("Amount is 0");
        yield return new TestCaseData("Title", "Description", -1.0m, DateTime.UtcNow, null).SetName(
            "Amount is negative");

        // Dates
        yield return new TestCaseData("Title", "Description", 10.0m, DateTime.UtcNow, DateTime.UtcNow.AddDays(-1))
            .SetName("End date is before start date");
    }

    [Test, TestCaseSource(nameof(CreateInvalidValidatorForUpdateExpense))]
    public void Validator_ShouldHaveErrors_WhenUpdateExpenseCommandIsInvalid(string title, string description,
        decimal amount, DateTime startDate, DateTime? endDate)
    {
        // Arrange
        var validator = new UpdatePlannedExpenseCommandValidator();
        var invalidCommand = new UpdatePlannedExpenseCommand
        {
            Title = title,
            Description = description,
            Amount = amount,
            StartDate = startDate,
            EndDate = endDate
        };

        // Act
        ValidationResult result = validator.Validate(invalidCommand);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}