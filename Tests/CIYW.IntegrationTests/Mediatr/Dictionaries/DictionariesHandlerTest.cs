using CIYW.IntegrationTests.Core;
using CommonModule.Shared.Enums;
using CommonModule.Shared.Responses.Dictionaries;
using CommonModule.Shared.Responses.Dictionaries.Models.Balances;
using CommonModule.Shared.Responses.Dictionaries.Models.Categories;
using CommonModule.Shared.Responses.Dictionaries.Models.Countries;
using CommonModule.Shared.Responses.Dictionaries.Models.Currencies;
using CommonModule.Shared.Responses.Dictionaries.Models.Expenses;
using CommonModule.Shared.Responses.Dictionaries.Models.Icons;
using Dictionaries.Domain;
using Dictionaries.Domain.Models.Categories;
using Dictionaries.Domain.Models.Countries;
using Dictionaries.Domain.Models.Currencies;
using Dictionaries.Domain.Models.Expenses;
using Dictionaries.Domain.Models.Icons;
using Dictionaries.Mediatr.Mediatr.Requests;
using Expenses.Domain.Models.Balances;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CIYW.IntegrationTests.Mediatr.Dictionaries;

[TestFixture]
public class DictionariesHandlerTest() : CommonIntegrationTestSetup()
{
    private SiteSettingsResponse _siteSettings;
    
    
    
    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        await base.OneTimeSetup();
        _siteSettings = await SiteSettings();
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedEmptyBalanceTypesDictionary_WhenRequestWithActualVersion(UserRoleEnum role)
    {
        await HandleValidDictionary<BalanceTypesRequest, BalanceEntity, BalanceTypeResponse>(role, _siteSettings.Version.BalanceType);
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedEmptyCategoriesDictionary_WhenRequestWithActualVersion(UserRoleEnum role)
    {
        await HandleValidDictionary<CategoriesRequest, CategoryEntity, CategoryResponse>(role, _siteSettings.Version.Category);
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedEmptyCountriesDictionary_WhenRequestWithActualVersion(UserRoleEnum role)
    {
        await HandleValidDictionary<CountriesRequest, CountryEntity, CountryResponse>(role, _siteSettings.Version.Country);
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedEmptyCurrenciesDictionary_WhenRequestWithActualVersion(UserRoleEnum role)
    {
        await HandleValidDictionary<CurrenciesRequest, CurrencyEntity, CurrencyResponse>(role, _siteSettings.Version.Currency);
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedEmptyFrequenciesDictionary_WhenRequestWithActualVersion(UserRoleEnum role)
    {
        await HandleValidDictionary<FrequenciesRequest, FrequencyEntity, FrequencyResponse>(role, _siteSettings.Version.Frequency);
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedEmptyIconCategoriesDictionary_WhenRequestWithActualVersion(UserRoleEnum role)
    {
        await HandleValidDictionary<IconCategoriesRequest, IconCategoryEntity, IconCategoryResponse>(role, _siteSettings.Version.IconCategory);
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedBalanceTypesDictionary_WhenRequestWithEmptyVersion(UserRoleEnum role)
    {
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            DictionariesDataContext dictionariesDataContext =
                scope.ServiceProvider.GetRequiredService<DictionariesDataContext>();
            await HandleValidDictionary<BalanceTypesRequest, BalanceEntity, BalanceTypeResponse>(role,
                string.Empty, await dictionariesDataContext.BalanceTypes.CountAsync());
        }
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedCategoriesDictionary_WhenRequestWithEmptyVersion(UserRoleEnum role)
    {
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            DictionariesDataContext dictionariesDataContext =
                scope.ServiceProvider.GetRequiredService<DictionariesDataContext>();  
            await HandleValidDictionary<CategoriesRequest, CategoryEntity, CategoryResponse>(role, string.Empty, await dictionariesDataContext.Categories.CountAsync(c => c.ParentId == null));
        }
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedCountriesDictionary_WhenRequestWithEmptyVersion(UserRoleEnum role)
    {
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            DictionariesDataContext dictionariesDataContext =
                scope.ServiceProvider.GetRequiredService<DictionariesDataContext>();
            await HandleValidDictionary<CountriesRequest, CountryEntity, CountryResponse>(role, string.Empty, await dictionariesDataContext.Countries.CountAsync());
        }
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedCurrenciesDictionary_WhenRequestWithEmptyVersion(UserRoleEnum role)
    {
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            DictionariesDataContext dictionariesDataContext =
                scope.ServiceProvider.GetRequiredService<DictionariesDataContext>();
            await HandleValidDictionary<CurrenciesRequest, CurrencyEntity, CurrencyResponse>(role, string.Empty, await dictionariesDataContext.Currencies.CountAsync());
        }
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedFrequenciesDictionary_WhenRequestWithEmptyVersion(UserRoleEnum role)
    {
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            DictionariesDataContext dictionariesDataContext =
                scope.ServiceProvider.GetRequiredService<DictionariesDataContext>();
            await HandleValidDictionary<FrequenciesRequest, FrequencyEntity, FrequencyResponse>(role, string.Empty, await dictionariesDataContext.Frequencies.CountAsync());
        }
    }
    
    [Test, TestCaseSource(nameof(CreateAllRolesTestCases))]
    public async Task Handle_ShouldReturnVersionedIconCategoriesDictionary_WhenRequestWithEmptyVersion(UserRoleEnum role)
    {
        using (var scope = TestApplicationFactory.Services.CreateScope())
        {
            DictionariesDataContext dictionariesDataContext =
                scope.ServiceProvider.GetRequiredService<DictionariesDataContext>();
            await HandleValidDictionary<IconCategoriesRequest, IconCategoryEntity, IconCategoryResponse>(role, string.Empty, await dictionariesDataContext.IconCategories.CountAsync());
        }
    }
}