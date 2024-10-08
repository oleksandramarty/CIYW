using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Auth;
using CommonModule.Shared.Responses.AuthGateway.Users;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using NSwag.Generation.AspNetCore;

namespace CommonModule.Facade;

public static class GraphQLModelExtension
{
    public static void AddGraphQLModels(this AspNetCoreOpenApiDocumentGeneratorSettings config, bool addAdditionalTypes = false)
    {
        if (addAdditionalTypes)
        {
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<SiteSettingsResponse>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<JwtTokenResponse>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<UserResponse>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<ExpenseResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<PlannedExpenseResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<UserProjectResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<UserAllowedProjectResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<BaseFilterRequest>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<UserAllowedProjectResponse>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<UserProjectResponse>());
        }
    }
}