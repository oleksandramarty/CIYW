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
public class UpdateFavoriteExpenseCommandHandlerTest : CommonIntegrationTestSetup
{
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldUpdateFavoriteExpense_WhenUpdateFavoriteExpenseCommandIsValid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role);
        UserProjectEntity userProject = user.UserProjects.First();
        Guid balanceId = userProject.Balances.First().Id;
        var favoriteExpenses = new Dictionary<int, int>
        {
            { 1, 1 }
        };
        await this.AddAllExpenses(
            user.User.Id,
            userProject.Id,
            balanceId,
            null,
            null,
            favoriteExpenses
        );

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();

            FavoriteExpenseEntity existingExpense = await expensesDataContext
                .FavoriteExpenses
                .FirstOrDefaultAsync(fe =>
                    fe.CreatedUserId == Options.CurrentUserEntity.User.Id &&
                    fe.UserProjectId == userProject.Id
                );

            IMediator mediator = new Mediator(scope.ServiceProvider);
            await mediator.Send(new UpdateFavoriteExpenseCommand
            {
                Id = existingExpense.Id,
                Title = "Updated Test",
                Description = "Updated Description",
                Limit = 500,
                CategoryId = 2,
                FrequencyId = 2,
                CurrencyId = 1,
                UserProjectId = userProject.Id,
                IconId = 1
            });

            // Assert
            FavoriteExpenseEntity? updatedExpense = await expensesDataContext
                .FavoriteExpenses
                .FirstOrDefaultAsync(fe =>
                    fe.CreatedUserId == Options.CurrentUserEntity.User.Id &&
                    fe.UserProjectId == userProject.Id &&
                    fe.Id == existingExpense.Id
                );

            updatedExpense.Should().NotBeNull();
            updatedExpense!.Title.Should().Be("Updated Test");
            updatedExpense!.Description.Should().Be("Updated Description");
            updatedExpense!.Limit.Should().Be(500);
            updatedExpense!.CategoryId.Should().Be(2);
            updatedExpense!.FrequencyId.Should().Be(2);
            updatedExpense!.CurrencyId.Should().Be(1);
            updatedExpense!.UserProjectId.Should().Be(userProject.Id);
            updatedExpense!.IconId.Should().Be(1);
        }
    }

    private static IEnumerable<TestCaseData> CreateInvalidValidatorForUpdateFavoriteExpense()
    {
        // Title
        yield return new TestCaseData(null, "Description", 10.0m, 1, 1, 1, 1).SetName("Title is null");
        yield return new TestCaseData(string.Empty, "Description", 10.0m, 1, 1, 1, 1).SetName("Title is empty");
        yield return new TestCaseData(new string('a', 51), "Description", 10.0m, 1, 1, 1, 1).SetName(
            "Title is 51 characters long");

        // Description
        yield return new TestCaseData("Title", new string('a', 101), 10.0m, 1, 1, 1, 1).SetName(
            "Description is 101 characters long");

        // Limit
        yield return new TestCaseData("Title", "Description", 0.0m, 1, 1, 1, 1).SetName("Limit is 0");
        yield return new TestCaseData("Title", "Description", -1.0m, 1, 1, 1, 1).SetName("Limit is negative");
    }

    [Test, TestCaseSource(nameof(CreateInvalidValidatorForUpdateFavoriteExpense))]
    public void Validator_ShouldHaveErrors_WhenUpdateFavoriteExpenseCommandIsInvalid(string title, string description,
        decimal limit, int categoryId, int frequencyId, int currencyId, int iconId)
    {
        // Arrange
        var validator = new UpdateFavoriteExpenseCommandValidator();
        var invalidCommand = new UpdateFavoriteExpenseCommand
        {
            Title = title,
            Description = description,
            Limit = limit,
            CategoryId = categoryId,
            FrequencyId = frequencyId,
            CurrencyId = currencyId,
            IconId = iconId
        };

        // Act
        ValidationResult result = validator.Validate(invalidCommand);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}