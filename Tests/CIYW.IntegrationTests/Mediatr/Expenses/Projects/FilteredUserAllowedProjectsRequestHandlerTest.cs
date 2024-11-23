using CIYW.IntegrationTests.Core;
using CIYW.IntegrationTests.Shared;
using CommonModule.Shared.Common;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using Expenses.Mediatr.Mediatr.Projects.Requests;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Expenses.Projects;

[TestFixture]
public class FilteredUserAllowedProjectsRequestHandlerTest() : CommonIntegrationTestSetup()
{
    private static IEnumerable<TestCaseData> CreateAllRolesFilteredUserAllowedProjectTestCases()
    {
        // Role, PageNumber, PageSize, IsFull, PredictedCount, PredictedPageNumber, PredictedPageSize
        yield return new TestCaseData(UserRoleEnum.User, -1, 5, false, 5, 1, 5).SetName("User role with -1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.User, 1, 5, false, 5, 1, 5).SetName("User role with 1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.User, 1, 150, false, 150, 1, 150).SetName("User role with 1 page should return 150 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.User, 201, 1, false, 0, 201, 1).SetName("User role with 51 page should return 0 projects for 51 page");
        yield return new TestCaseData(UserRoleEnum.User, 1, 5, true, 150, 1, 150).SetName("User role with 1 page should return 150 projects for 1 page with isFull true");
        
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, -1, 5, false, 5, 1, 5).SetName("Technical Support role with -1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 5, false, 5, 1, 5).SetName("Technical Support role with 1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 150, false, 150, 1, 150).SetName("Technical Support role with 1 page should return 150 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 201, 1, false, 0, 201, 1).SetName("Technical Support role with 51 page should return 0 projects for 51 page");
        yield return new TestCaseData(UserRoleEnum.TechnicalSupport, 1, 5, true, 150, 1, 150).SetName("Technical Support role with 1 page should return 150 projects for 1 page with isFull true");
        
        yield return new TestCaseData(UserRoleEnum.Admin, -1, 5, false, 5, 1, 5).SetName("Admin role with -1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 5, false, 5, 1, 5).SetName("Admin role with 1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 150, false, 150, 1, 150).SetName("Admin role with 1 page should return 150 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 201, 1, false, 0, 201, 1).SetName("Admin role with 51 page should return 0 projects for 51 page");
        yield return new TestCaseData(UserRoleEnum.Admin, 1, 5, true, 150, 1, 150).SetName("Admin role with 1 page should return 150 projects for 1 page with isFull true");
        
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, -1, 5, false, 5, 1, 5).SetName("Super Admin role with -1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 5, false, 5, 1, 5).SetName("Super Admin role with 1 page should return 5 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 150, false, 150, 1, 150).SetName("Super Admin role with 1 page should return 150 projects for 1 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 201, 1, false, 0, 201, 1).SetName("Super Admin role with 51 page should return 0 projects for 51 page");
        yield return new TestCaseData(UserRoleEnum.SuperAdmin, 1, 5, true, 150, 1, 150).SetName("Super Admin role with 1 page should return 150 projects for 1 page with isFull true");
    }
    
    
    [Test, TestCaseSource(nameof(CreateAllRolesFilteredUserAllowedProjectTestCases))]
    public async Task Handle_ShouldReturnFilteredUserAllowedProjects_WhenRequestIsValid(
        UserRoleEnum role,
        int pageNumber,
        int pageSize,
        bool isFull,
        int predictedCount,
        int predictedPageNumber,
        int predictedPageSize)
    {
        // Arrange
        await this.SignOutUserIfExist();
        IntegrationTestUserEntity user = await this.CreateTestUser(role, 1, 1);
        await AddUserAllowedProjects(user.User.Id,  200);

        // Act
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            IMediator mediator = new Mediator(scope.ServiceProvider);
            FilteredUserAllowedProjectsRequest request = new FilteredUserAllowedProjectsRequest
            {
                Paginator = new PaginatorEntity(pageNumber, pageSize, isFull),
                AmountRange = null,
                DateRange = null,
                Query = null,
                Sort = new BaseSortableRequest(ColumnEnum.CreatedAt, OrderDirectionEnum.Desc)
            };

            FilteredListResponse<UserAllowedProjectResponse> response = await mediator.Send(request);

            // Assert
            response.Should().NotBeNull();
            response.Paginator.Should().NotBeNull();
            response.Paginator!.PageNumber.Should().Be(predictedPageNumber);
            response.Paginator!.PageSize.Should().Be(predictedPageSize);
            response.Paginator!.IsFull.Should().Be(isFull);
            response.TotalCount.Should().Be(200);
            response.Entities.Should().NotBeNull();
            response.Entities.Count().Should().Be(predictedCount);
        }
    }
}