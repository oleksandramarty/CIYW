using CommonModule.Shared;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Requests.Base;
using CommonModule.Shared.Responses.Auth;
using CommonModule.Shared.Responses.AuthGateway.Users;
using CommonModule.Shared.Responses.Base;
using CommonModule.Shared.Responses.Dictionaries;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Balances;
using CommonModule.Shared.Responses.Expenses.Models.Expenses;
using CommonModule.Shared.Responses.Expenses.Models.Projects;
using CommonModule.Shared.Responses.Localizations;
using CommonModule.Shared.Responses.Localizations.Models.Locales;
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
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<BaseFilterRequest>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<LocalizationsResponse>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<ExpenseResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<PlannedExpenseResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<UserProjectResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<FilteredListResponse<UserAllowedProjectResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<VersionedListResponse<CurrencyResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<VersionedListResponse<FrequencyResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<VersionedListResponse<LocaleResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<VersionedListResponse<CountryResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<VersionedListResponse<CategoryResponse>>());
            config.DocumentProcessors.Add(new AddAdditionalTypeProcessor<VersionedListResponse<BalanceTypeResponse>>());
        }
    }
}