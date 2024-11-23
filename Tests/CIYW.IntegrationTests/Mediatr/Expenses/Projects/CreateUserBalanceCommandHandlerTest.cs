using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Core.Exceptions;
using CommonModule.Core.Extensions;
using CommonModule.Shared.Constants;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Base;
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
public class CreateUserBalanceCommandHandlerTest() : CommonIntegrationTestSetup()
{
    private static IEnumerable<TestCaseData> CreateAllRolesUserBalanceTestCases()
    {
        yield return new TestCaseData(UserRoleEnum.User, 0).SetName("User role with 0 user balances");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 0).SetName(
            "Technical Support role with 0 user balances");
        yield return new TestCaseData(UserRoleEnum.Admin, 0).SetName("Admin role with 0 user balances");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 0).SetName("Super Admin role with 0 user balances");

        yield return new TestCaseData(UserRoleEnum.User, 1).SetName("User role with 1 user balance");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1).SetName(
            "Technical Support role with 1 user balance");
        yield return new TestCaseData(UserRoleEnum.Admin, 1).SetName("Admin role with 1 user balance");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1).SetName("Super Admin role with 1 user balance");

        yield return new TestCaseData(UserRoleEnum.User, 2).SetName("User role with 2 user balances");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 2).SetName(
            "Technical Support role with 2 user balances");
        yield return new TestCaseData(UserRoleEnum.Admin, 2).SetName("Admin role with 2 user balances");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 2).SetName("Super Admin role with 2 user balances");
    }

    [Test, TestCaseSource(nameof(CreateAllRolesUserBalanceTestCases))]
    public async Task Handle_ShouldReturnBalanceId_WhenCreateUserBalanceCommandIsValid(
        UserRoleEnum role,
        int userBalancesCount
    )
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role, 1, userBalancesCount);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();

            IMediator mediator = new Mediator(scope.ServiceProvider);
            BaseEntityIdResponse<Guid> response = await mediator.Send(new CreateUserBalanceCommand
            {
                UserProjectId = user.UserProjects.First().Id,
                CurrencyId = IntegrationTestConstants.DefaultCurrencyId,
                IconId = IntegrationTestConstants.DefaultIconId,
                Title = "Test",
                IsActive = true,
                BalanceTypeId = 1
            });

            // Assert
            List<BalanceEntity> userBalances = await expensesDataContext.Balances
                .Where(up => up.UserId == Options.CurrentUserEntity.User.Id).ToListAsync();
            BalanceEntity? createdUserBalance = userBalances.FirstOrDefault(up => up.Id == response.Id);

            response.Should().NotBeNull();
            response.Id.Should().NotBeEmpty();
            createdUserBalance.Should().NotBeNull();
            createdUserBalance!.Title.Should().Be("Test");
            createdUserBalance!.IsActive.Should().BeTrue();
            userBalances.Should().NotBeNull();
            userBalances.Should().NotBeEmpty();
            userBalances.Count().Should().Be(userBalancesCount + 1);
        }
    }

    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnException_WhenCreateUserBalanceCommandIsInvalid(UserRoleEnum role)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role, 1, 3);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            ExpensesDataContext expensesDataContext = scope.ServiceProvider.GetRequiredService<ExpensesDataContext>();

            IMediator mediator = new Mediator(scope.ServiceProvider);
            await TestUtilities
                .Handle_InvalidCommand<CreateUserBalanceCommand, BaseEntityIdResponse<Guid>, BusinessException>(
                    mediator,
                    new CreateUserBalanceCommand
                    {
                        UserProjectId = user.UserProjects.First().Id,
                        CurrencyId = IntegrationTestConstants.DefaultCurrencyId,
                        IconId = IntegrationTestConstants.DefaultIconId,
                        Title = "Test",
                        IsActive = true,
                        BalanceTypeId = 1
                    },
                    ErrorMessages.UserProjectLimitExceeded);
        }
    }

    private static IEnumerable<TestCaseData> CreateInvalidValidatorForSignUp()
    {
        // Title
        yield return new TestCaseData(null).SetName("Title is null");
        yield return new TestCaseData(string.Empty).SetName("Title is empty");
        yield return
            new TestCaseData(StringExtension.GenerateRandomString(101)).SetName("Title is 101 characters long");
    }

    [Test, TestCaseSource(nameof(CreateInvalidValidatorForSignUp))]
    public void Validator_ShouldHaveErrors_WhenSignUpRequestIsInvalid(string title)
    {
        // Arrange
        var validator = new CreateUserBalanceCommandValidator();
        var invalidCommand = new CreateUserBalanceCommand
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