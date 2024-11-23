using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using Expenses.Domain.Models.Projects;
using Expenses.Mediatr.Mediatr.Expenses.Requests;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Expenses.Expenses;

[TestFixture]
public class FilteredFavoriteExpensesRequestHandlerTest() : CommonIntegrationTestSetup()
{
        private static IEnumerable<TestCaseData> CreateAllRolesFilteredFavoriteExpensesTestCases()
    {
        // Role, PageNumber, PageSize, IsFull, PredictedCount, PredictedPageNumber, PredictedPageSize, categoriesIds Array
        yield return new TestCaseData(UserRoleEnum.User, -1, 5, false, 5, 1, 5, new int[] {}).SetName("User role with -1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.User, 1, 5, false, 5, 1, 5, new int[] {}).SetName("User role with 1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.User, 1, 150, false, 150, 1, 150, new int[] {}).SetName("User role with 1 page should return 150 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.User, 201, 1, false, 0, 201, 1, new int[] {}).SetName("User role with 51 page should return 0 favorite expenses for 51 page");
        yield return new TestCaseData(UserRoleEnum.User, 1, 5, true, 150, 1, 150, new int[] {}).SetName("User role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.User, 1, 5, true, 90, 1, 150, new int[] {1, 3, 5}).SetName("User role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.User, 1, 5, true, 10, 1, 150, new int[] {1}).SetName("User role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, -1, 5, false, 5, 1, 5, new int[] {}).SetName("Technical Support role with -1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 5, false, 5, 1, 5, new int[] {}).SetName("Technical Support role with 1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 150, false, 150, 1, 150, new int[] {}).SetName("Technical Support role with 1 page should return 150 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 201, 1, false, 0, 201, 1, new int[] {}).SetName("Technical Support role with 51 page should return 0 favorite expenses for 51 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 5, true, 150, 1, 150, new int[] {}).SetName("Technical Support role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 5, true, 90, 1, 150, new int[] {1, 3, 5}).SetName("Technical Support role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 5, true, 10, 1, 150, new int[] {1}).SetName("Technical Support role with 1 page should return 150 favorite expenses for 1 page with isFull true");

        yield return new TestCaseData(UserRoleEnum.Admin, -1, 5, false, 5, 1, 5, new int[] {}).SetName("Admin role with -1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 5, false, 5, 1, 5, new int[] {}).SetName("Admin role with 1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 150, false, 150, 1, 150, new int[] {}).SetName("Admin role with 1 page should return 150 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 201, 1, false, 0, 201, 1, new int[] {}).SetName("Admin role with 51 page should return 0 favorite expenses for 51 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 5, true, 150, 1, 150, new int[] {}).SetName("Admin role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 5, true, 90, 1, 150, new int[] {1, 3, 5}).SetName("Admin role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 5, true, 10, 1, 150, new int[] {1}).SetName("Admin role with 1 page should return 150 favorite expenses for 1 page with isFull true");

        yield return new TestCaseData(UserRoleEnum.SuperAdmin, -1, 5, false, 5, 1, 5, new int[] {}).SetName("Super Admin role with -1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 5, false, 5, 1, 5, new int[] {}).SetName("Super Admin role with 1 page should return 5 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 150, false, 150, 1, 150, new int[] {}).SetName("Super Admin role with 1 page should return 150 favorite expenses for 1 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 201, 1, false, 0, 201, 1, new int[] {}).SetName("Super Admin role with 51 page should return 0 favorite expenses for 51 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 5, true, 150, 1, 150, new int[] {}).SetName("Super Admin role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 5, true, 90, 1, 150, new int[] {1, 3, 5}).SetName("Super Admin role with 1 page should return 150 favorite expenses for 1 page with isFull true");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 5, true, 10, 1, 150, new int[] {1}).SetName("Super Admin role with 1 page should return 150 favorite expenses for 1 page with isFull true");
    }

    [Test, TestCaseSource(nameof(CreateAllRolesFilteredFavoriteExpensesTestCases))]
    public async Task Handle_ShouldReturnFilteredFavoriteExpenses_WhenRequestIsValid(
        UserRoleEnum role,
        int pageNumber,
        int pageSize,
        bool isFull,
        int predictedCount,
        int predictedPageNumber,
        int predictedPageSize,
        int[] categoriesIds)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role, 1, 1);
        UserProjectEntity userProject = user.UserProjects.First();
        Guid balanceId = userProject.Balances.First().Id;
        var favoriteExpenses = new Dictionary<int, int>
        {
            { 1, 10 },
            { 2, 20 },
            { 3, 30 },
            { 4, 40 },
            { 5, 50 },
            { 6, 50 },
        };
        await this.AddAllExpenses(user.User.Id, userProject.Id, balanceId, null, null, favoriteExpenses);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            IMediator mediator = new Mediator(scope.ServiceProvider);
            FilteredFavoriteExpensesRequest request = new FilteredFavoriteExpensesRequest
            {
                Paginator = new PaginatorEntity(pageNumber, pageSize, isFull),
                AmountRange = null,
                DateRange = null,
                Query = null,
                Sort = new BaseSortableRequest(ColumnEnum.CreatedAt, OrderDirectionEnum.Desc),
                UserProjectId = userProject.Id,
                CategoryIds = categoriesIds.Any() ? new BaseFilterIdsRequest<int> { Ids = categoriesIds.ToList() } : null
            };

            FilteredListResponse<FavoriteExpenseResponse> response = await mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Paginator.Should().NotBeNull();
            response.Paginator!.PageNumber.Should().Be(predictedPageNumber);
            response.Paginator!.PageSize.Should().Be(predictedPageSize);
            response.Paginator!.IsFull.Should().Be(isFull);
            response.TotalCount.Should().Be(categoriesIds.Any() ? favoriteExpenses.Where(e => categoriesIds.Contains(e.Key)).Sum(e => e.Value) : 200);
            response.Entities.Should().NotBeNull();
            response.Entities.Count().Should().Be(predictedCount);
        }
    }
}