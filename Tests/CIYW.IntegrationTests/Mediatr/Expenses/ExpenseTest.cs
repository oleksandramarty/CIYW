using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
using Expenses.Domain;
using Expenses.Domain.Models.Balances;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Commands;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Expenses;

[TestFixture]
public class ExpenseTest() : CommonIntegrationTestSetup(UserRoleEnum.User)
{
    private static IEnumerable<TestCaseData> CreateExpensesTestCases()
    {
        // categories IDS
        // Incomes
        // 69 - Salary
        // 102 - Other incomes
        // Expenses
        // 17 - Groceries
        // 58 - Pet food
        // 60 - Pet insurance
        // 2 - Rent
        // categoryId, amountByCreate, amountByUpdate, isDelete, predictedAmountAfterCreate, predictedAmountAfterUpdate
        yield return new TestCaseData(69, 3500.0m, 3200.0m, true, 3500.0m, 3200.0m).SetName("CRUD for Salary with delete");
        yield return new TestCaseData(102, 1000.0m, 800.0m, true, 1000.0m, 800.0m).SetName("CRUD for Other incomes with delete");
        yield return new TestCaseData(17, 200.0m, 350.0m, true, -200.0m, -350.0m).SetName("CRUD for Groceries with delete");
        yield return new TestCaseData(58, 50.0m, 100.0m, true, -50.0m, -100.0m).SetName("CRUD for Pet food with delete");
        yield return new TestCaseData(60, 100.0m, 200.0m, true, -100.0m, -200.0m).SetName("CRUD for Pet insurance with delete");
        yield return new TestCaseData(2, 1000.0m, 1200.0m, true, -1000.0m, -1200.0m).SetName("CRUD for Rent with delete");

        yield return new TestCaseData(69, 3500.0m, 3200.0m, false, 3500.0m, 3200.0m).SetName("CRUD for Salary without delete");
        yield return new TestCaseData(102, 1000.0m, 800.0m, false, 4200.0m, 4000.0m).SetName("CRUD for Other incomes without delete");
        yield return new TestCaseData(17, 200.0m, 350.0m, false, 3800.0m, 3650.0m).SetName("CRUD for Groceries without delete");
        yield return new TestCaseData(58, 50.0m, 100.0m, false, 3600.0m, 3550.0m).SetName("CRUD for Pet food without delete");
        yield return new TestCaseData(60, 100.0m, 200.0m, false, 3450.0m, 3350.0m).SetName("CRUD for Pet insurance without delete");
        yield return new TestCaseData(2, 1000.0m, 1200.0m, false, 2350.0m, 2150.0m).SetName("CRUD for Rent without delete");
    }

    [Test, TestCaseSource(nameof(CreateExpensesTestCases))]
    public async Task Handle_ShouldSetUpValidBalance_WhenCommandIsValid(
        int categoryId,
        decimal amountByCreate,
        decimal amountByUpdate,
        bool isDelete,
        decimal predictedAmountAfterCreate,
        decimal predictedAmountAfterUpdate
    )
    {
        // Arrange
        IntegrationTestUserEntity? userHelper = Options.CurrentUserEntity;

        UserProjectEntity userProject = userHelper.UserProjects.First();
        BalanceEntity balance = userProject.Balances.First();

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();

            IMediator mediator = new Mediator(scope.ServiceProvider);
            BaseEntityIdResponse<Guid> response = await mediator.Send(new CreateExpenseCommand()
            {
                Title = "Test",
                Amount = amountByCreate,
                Date = DateTime.UtcNow,
                UserProjectId = userProject.Id,
                BalanceId = balance.Id,
                CategoryId = categoryId,
                FavoriteExpenseId = null
            });

            // Assert
            decimal? balanceAfterCreate = await expensesDataContext.Balances
                .Where(b => b.Id == balance.Id)
                .Select(b => b.Amount)
                .FirstAsync();

            balanceAfterCreate.Should().NotBeNull();
            balanceAfterCreate.Should().Be(predictedAmountAfterCreate);

            // Act
            await mediator.Send(new UpdateExpenseCommand()
            {
                Id = response.Id,
                Title = "Test",
                Amount = amountByUpdate,
                Date = DateTime.UtcNow,
                BalanceId = balance.Id,
                CategoryId = categoryId
            });

            // Assert
            decimal? balanceAfterUpdate = await expensesDataContext.Balances
                .Where(b => b.Id == balance.Id)
                .Select(b => b.Amount)
                .FirstAsync();

            balanceAfterUpdate.Should().NotBeNull();
            balanceAfterUpdate.Should().Be(predictedAmountAfterUpdate);

            if (isDelete)
            {
                // Act
                await mediator.Send(new RemoveExpenseCommand()
                {
                    Id = response.Id
                });

                // Assert
                decimal? balanceAfterRemove = await expensesDataContext.Balances
                    .Where(b => b.Id == balance.Id)
                    .Select(b => b.Amount)
                    .FirstAsync();

                balanceAfterRemove.Should().NotBeNull();
                balanceAfterRemove.Should().Be(0);
            }
        }
    }
}