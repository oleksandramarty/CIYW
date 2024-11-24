using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Exceptions;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
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
public class CreatePlannedExpenseCommandHandlerTest() : CommonIntegrationTestSetup()
{
    private static IEnumerable<TestCaseData> CreateAllRolesPlannedExpensesTestCases()
    {
        foreach (UserRoleEnum role in Enum.GetValues(typeof(UserRoleEnum)))
        {
            for (var i = 0; i < 10; i++)
            {
                yield return new TestCaseData(role, i).SetName($"{role} role with {i} planned expenses");
            }
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesPlannedExpensesTestCases))]
    public async Task Handle_ShouldReturnPlannedExpenseId_WhenCreatePlannedExpenseCommandIsValid(
        UserRoleEnum role,
        int plannedExpensesCount
    )
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role);
        UserProjectEntity userProject = user.UserProjects.First();
        Guid balanceId = userProject.Balances.First().Id;
        var plannedExpenses = new Dictionary<int, int>
        {
            { 1, plannedExpensesCount }
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

            IMediator mediator = new Mediator(scope.ServiceProvider);
            BaseEntityIdResponse<Guid> response = await mediator.Send(new CreatePlannedExpenseCommand
            {
                Title = "Test",
                Description = null,
                Amount = 100,
                CategoryId = 1,
                BalanceId = balanceId,
                StartDate = DateTime.UtcNow,
                EndDate = null,
                UserProjectId = userProject.Id,
                FrequencyId = 1
            });

            // Assert
            PlannedExpenseEntity? plannedExpenseEntity = await expensesDataContext
                .PlannedExpenses
                .FirstOrDefaultAsync(fe =>
                    fe.CreatedUserId == Options.CurrentUserEntity.User.Id &&
                    fe.UserProjectId == userProject.Id &&
                    fe.Id == response.Id
                );

            response.Should().NotBeNull();
            response.Id.Should().NotBeEmpty();
            plannedExpenseEntity.Should().NotBeNull();
            plannedExpenseEntity!.Title.Should().Be("Test");
            plannedExpenseEntity!.Description.Should().BeNull();
            plannedExpenseEntity!.Amount.Should().Be(100);
            plannedExpenseEntity!.CategoryId.Should().Be(1);
            plannedExpenseEntity!.BalanceId.Should().Be(balanceId);
            plannedExpenseEntity!.EndDate.Should().BeNull();
            plannedExpenseEntity!.UserProjectId.Should().Be(userProject.Id);
            plannedExpenseEntity!.FrequencyId.Should().Be(1);
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenCreatePlannedExpenseCommandIsInvalid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role);
        UserProjectEntity userProject = user.UserProjects.First();
        Guid balanceId = userProject.Balances.First().Id;
        var plannedExpenses = new Dictionary<int, int>
        {
            { 1, 10 }
        };
        await this.AddAllExpenses(
            user.User.Id,
            userProject.Id,
            balanceId,
            null,
            plannedExpenses);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            IMediator mediator = new Mediator(scope.ServiceProvider);
            await TestUtilities
                .Handle_InvalidCommand<CreatePlannedExpenseCommand, BaseEntityIdResponse<Guid>, BusinessException>(
                    mediator,
                    new CreatePlannedExpenseCommand
                    {
                        Title = "Test",
                        Description = null,
                        Amount = 100,
                        CategoryId = 1,
                        BalanceId = balanceId,
                        StartDate = DateTime.UtcNow,
                        EndDate = null,
                        UserProjectId = userProject.Id,
                        FrequencyId = 1
                    },
                    ErrorMessages.UserProjectLimitExceeded);
        }
    }

    private static IEnumerable<TestCaseData> CreateInvalidValidatorForCreateExpense()
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

    [Test, TestCaseSource(nameof(CreateInvalidValidatorForCreateExpense))]
    public void Validator_ShouldHaveErrors_WhenCreateExpenseCommandIsInvalid(string title, string description,
        decimal amount, DateTime startDate, DateTime? endDate)
    {
        // Arrange
        var validator = new CreatePlannedExpenseCommandValidator();
        var invalidCommand = new CreatePlannedExpenseCommand
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