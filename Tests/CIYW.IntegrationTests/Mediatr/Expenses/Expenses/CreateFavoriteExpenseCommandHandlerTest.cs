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
public class CreateFavoriteExpenseCommandHandlerTest() : CommonIntegrationTestSetup()
{
    private static IEnumerable<TestCaseData> CreateAllRolesFavoriteExpensesTestCases()
    {
        foreach (UserRoleEnum role in Enum.GetValues(typeof(UserRoleEnum)))
        {
            for (var i = 0; i < 10; i++)
            {
                yield return new TestCaseData(role, i).SetName($"{role} role with {i} favorite expenses");
            }
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesFavoriteExpensesTestCases))]
    public async Task Handle_ShouldReturnFavoriteExpenseId_WhenCreateFavoriteExpenseCommandIsValid(
        UserRoleEnum role,
        int favoriteExpensesCount
    )
    {
        // Arrange
        await SignOutUserIfExist();
        IntegrationTestUserEntity user = await CreateTestUser(role);
        UserProjectEntity userProject = user.UserProjects.First();
        Guid balanceId = userProject.Balances.First().Id;
        var favoriteExpenses = new Dictionary<int, int>
        {
            { 1, favoriteExpensesCount }
        };
        await AddAllExpenses(
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

            IMediator mediator = new Mediator(scope.ServiceProvider);
            BaseEntityIdResponse<Guid> response = await mediator.Send(new CreateFavoriteExpenseCommand
            {
                Title = "Test",
                Limit = 100,
                CurrencyId = IntegrationTestConstants.DefaultCurrencyId,
                UserProjectId = userProject.Id,
                IconId = IntegrationTestConstants.DefaultIconId
            });

            // Assert
            FavoriteExpenseEntity? favoriteExpenseEntity = await expensesDataContext
                .FavoriteExpenses
                .FirstOrDefaultAsync(fe =>
                    fe.CreatedUserId == Options.CurrentUserEntity.User.Id &&
                    fe.UserProjectId == userProject.Id &&
                    fe.Id == response.Id
                );

            response.Should().NotBeNull();
            response.Id.Should().NotBeEmpty();
            favoriteExpenseEntity.Should().NotBeNull();
            favoriteExpenseEntity!.Title.Should().Be("Test");
            favoriteExpenseEntity!.Limit.Should().Be(100);
            favoriteExpenseEntity!.CurrencyId.Should().Be(IntegrationTestConstants.DefaultCurrencyId);
            favoriteExpenseEntity!.UserProjectId.Should().Be(userProject.Id);
            favoriteExpenseEntity!.IconId.Should().Be(IntegrationTestConstants.DefaultIconId);
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenCreateFavoriteExpenseCommandIsInvalid(UserRoleEnum role)
    {
        // Arrange
        await SignOutUserIfExist();
        IntegrationTestUserEntity user = await CreateTestUser(role);
        UserProjectEntity userProject = user.UserProjects.First();
        Guid balanceId = userProject.Balances.First().Id;
        var favoriteExpenses = new Dictionary<int, int>
        {
            { 1, 10 }
        };
        await AddAllExpenses(
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
            IMediator mediator = new Mediator(scope.ServiceProvider);
            await TestUtilities
                .Handle_InvalidCommand<CreateFavoriteExpenseCommand, BaseEntityIdResponse<Guid>, BusinessException>(
                    mediator,
                    new CreateFavoriteExpenseCommand
                    {
                        Title = "Test",
                        Limit = 100,
                        CurrencyId = IntegrationTestConstants.DefaultCurrencyId,
                        UserProjectId = userProject.Id,
                        IconId = IntegrationTestConstants.DefaultIconId
                    },
                    ErrorMessages.UserProjectLimitExceeded);
        }
    }

    private static IEnumerable<TestCaseData> CreateInvalidValidatorForCreateExpense()
    {
        // Title
        yield return new TestCaseData(null, "Description", 10.0m).SetName("Title is null");
        yield return new TestCaseData(string.Empty, "Description", 10.0m).SetName("Title is empty");
        yield return new TestCaseData(new string('a', 51), "Description", 10.0m).SetName("Title is 51 characters long");

        // Description
        yield return new TestCaseData("Title", new string('a', 101), 10.0m).SetName("Description is 101 characters long");

        // Amount
        yield return new TestCaseData("Title", "Description", 0.0m).SetName("Amount is 0");
        yield return new TestCaseData("Title", "Description", -1.0m).SetName("Amount is negative");
    }

    [Test, TestCaseSource(nameof(CreateInvalidValidatorForCreateExpense))]
    public void Validator_ShouldHaveErrors_WhenCreateExpenseCommandIsInvalid(string title, string description,
        decimal amount)
    {
        // Arrange
        var validator = new CreateExpenseCommandValidator();
        var invalidCommand = new CreateExpenseCommand
        {
            Title = title,
            Description = description,
            Amount = amount
        };

        // Act
        ValidationResult result = validator.Validate(invalidCommand);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}