import { Injectable } from "@angular/core";
import {catchError, forkJoin, of, take, tap} from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { handleApiError } from "../helpers/rxjs.helper";
import {Dictionary, DictionaryDataItems, DictionaryMap} from "../models/common/dictionary.model";
import {LocalStorageService} from "./local-storage.service";
import {SiteSettingsService} from "./site-settings.service";
import {DataItem} from "../models/common/data-item.model";
import {LocalizationService} from "./localization.service";
import {
    BalanceTypeResponse,
    CategoryResponse,
    CountryResponse,
    CurrencyResponse,
    FrequencyResponse, IconCategoryResponse, IconResponse,
    LocaleResponse, VersionedListResponseOfBalanceTypeResponse, VersionedListResponseOfCategoryResponse,
    VersionedListResponseOfCountryResponse,
    VersionedListResponseOfCurrencyResponse,
    VersionedListResponseOfFrequencyResponse, VersionedListResponseOfIconCategoryResponse,
    VersionedListResponseOfLocaleResponse
} from "../api-models/common.models";
import {GraphQlLocalizationsService} from "../graph-ql/services/graph-ql-localizations.service";
import {GraphQlDictionariesService} from "../graph-ql/services/graph-ql-dictionaries.service";
import {extractClassName} from "../helpers/dom.helper";
import {LoaderService} from "./loader.service";

@Injectable({
    providedIn: 'root'
})
export class DictionaryService {
    _dictionaries: Dictionary | undefined;

    _dataItems: DictionaryDataItems | undefined;

    _countriesMap: DictionaryMap<number, CountryResponse> | undefined;
    _currenciesMap: DictionaryMap<number, CurrencyResponse> | undefined;
    _categoriesMap: DictionaryMap<number, CategoryResponse> | undefined;
    _localesMap: DictionaryMap<number, LocaleResponse> | undefined;
    _frequenciesMap: DictionaryMap<number, FrequencyResponse> | undefined;
    _balanceTypesMap: DictionaryMap<number, BalanceTypeResponse> | undefined;
    _iconCategoryMap: DictionaryMap<number, IconCategoryResponse> | undefined;
    _iconMap: DictionaryMap<number, IconResponse> | undefined;

    private readonly _importantCurrencies = ['USD', 'EUR', 'GBP', 'CAD', 'MDL', 'UAH'];

    get dictionaries(): Dictionary | undefined {
        return this._dictionaries;
    }

    get dataItems(): DictionaryDataItems | undefined {
        return this._dataItems;
    }

    get iconMap(): DictionaryMap<number, IconResponse> | undefined {
        if (!this._iconMap && this.dictionaries?.iconCategories?.items) {
            this._iconMap = new DictionaryMap<number, IconResponse>();

            for (const icon of this.dictionaries.iconCategories.items) {
                for (const iconItem of icon.icons) {
                    this._iconMap.set(iconItem.id, iconItem);
                }
            }
        }

        return this._iconMap;
    }

    set iconMap(value: VersionedListResponseOfIconCategoryResponse | undefined) {
        if (!!value) {
            this._iconMap = new DictionaryMap<number, IconResponse>();

            for (const icon of value.items) {
                for (const iconItem of icon.icons) {
                    this._iconMap.set(iconItem.id, iconItem);
                }
            }
        }
    }

    get iconCategoryMap(): DictionaryMap<number, IconCategoryResponse> | undefined {
        if (!this._iconCategoryMap && this.dictionaries?.iconCategories?.items) {
            this._iconCategoryMap = new DictionaryMap<number, IconCategoryResponse>();

            for (const iconCategory of this.dictionaries.iconCategories.items) {
                this._iconCategoryMap.set(iconCategory.id, iconCategory);
            }
        }

        return this._iconCategoryMap;
    }

    set iconCategoryMap(value: VersionedListResponseOfIconCategoryResponse | undefined) {
        if (!!value) {
            this._iconCategoryMap = new DictionaryMap<number, IconCategoryResponse>();

            for (const iconCategory of value.items) {
                this._iconCategoryMap.set(iconCategory.id, iconCategory);
            }
        }
    }

    get countriesMap(): DictionaryMap<number, CountryResponse> | undefined {
        if (!this._countriesMap && this.dictionaries?.countries?.items) {
            this._countriesMap = new DictionaryMap<number, CountryResponse>();

            for (const country of this.dictionaries.countries.items) {
                this._countriesMap.set(country.id, country);
            }
        }
        return this._countriesMap;
    }

    set countriesMap(value: VersionedListResponseOfCountryResponse | undefined) {
        if (!!value) {
            this._countriesMap = new DictionaryMap<number, CountryResponse>();

            for (const country of value.items) {
                this._countriesMap.set(country.id, country);
            }
        }
    }

    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        if (!this._currenciesMap && this.dictionaries?.currencies?.items) {
            this._currenciesMap = new DictionaryMap<number, CurrencyResponse>();

            for (const currency of this.dictionaries.currencies.items) {
                this._currenciesMap.set(currency.id, currency);
            }
        }
        return this._currenciesMap;
    }

    set currenciesMap(value: VersionedListResponseOfCurrencyResponse | undefined) {
        if (!!value) {
            this._currenciesMap = new DictionaryMap<number, CurrencyResponse>();

            for (const currency of value.items) {
                this._currenciesMap.set(currency.id, currency);
            }
        }
    }

    get categoriesMap(): DictionaryMap<number, CategoryResponse> | undefined {
        if (!this._categoriesMap && this.dictionaries?.categories?.items) {
            this._categoriesMap = new DictionaryMap<number, CategoryResponse>();

            for (const category of this.mapCategoriesToFlat(this.dictionaries.categories.items)) {
                this._categoriesMap.set(category.id, category);
            }
        }
        return this._categoriesMap;
    }

    set categoriesMap(value: VersionedListResponseOfCategoryResponse | undefined) {
        if (!!value) {
            this._categoriesMap = new DictionaryMap<number, CategoryResponse>();

            for (const category of this.mapCategoriesToFlat(value.items)) {
                this._categoriesMap.set(category.id, category);
            }
        }
    }

    get localesMap(): DictionaryMap<number, LocaleResponse> | undefined {
        if (!this._localesMap && this.dictionaries?.locales?.items) {
            this._localesMap = new DictionaryMap<number, LocaleResponse>();

            for (const locale of this.dictionaries.locales.items) {
                this._localesMap.set(locale.id, locale);
            }
        }
        return this._localesMap;
    }

    set localesMap(value: VersionedListResponseOfLocaleResponse | undefined) {
        if (!!value) {
            this._localesMap = new DictionaryMap<number, LocaleResponse>();

            for (const locale of value.items) {
                this._localesMap.set(locale.id, locale);
            }
        }
    }

    get frequenciesMap(): DictionaryMap<number, FrequencyResponse> | undefined {
        if (!this._frequenciesMap && this.dictionaries?.frequencies?.items) {
            this._frequenciesMap = new DictionaryMap<number, FrequencyResponse>();

            for (const frequency of this.dictionaries.frequencies.items) {
                this._frequenciesMap.set(frequency.id, frequency)
            }
        }

        return this._frequenciesMap;
    }

    set frequenciesMap(value: VersionedListResponseOfFrequencyResponse | undefined) {
        if (!!value) {
            this._frequenciesMap = new DictionaryMap<number, FrequencyResponse>();

            for (const frequency of value.items) {
                this._frequenciesMap.set(frequency.id, frequency);
            }
        }
    }

    get currentLocale(): LocaleResponse | undefined {
        return this?.dictionaries?.locales?.items?.
        find(locale => locale.isoCode === this.siteSettingsService?.siteSettings?.locale);
    }

    get balanceTypesMap(): DictionaryMap<number, BalanceTypeResponse> | undefined {
        if (!this._balanceTypesMap && this.dictionaries?.balanceTypes?.items) {
            this._balanceTypesMap = new DictionaryMap<number, BalanceTypeResponse>();

            for (const balanceType of this.dictionaries.balanceTypes.items) {
                this._balanceTypesMap.set(balanceType.id, balanceType);
            }
        }

        return this._balanceTypesMap;
    }

    set balanceTypesMap(value: VersionedListResponseOfBalanceTypeResponse | undefined) {
        if (!!value) {
            this._balanceTypesMap = new DictionaryMap<number, BalanceTypeResponse>();

            for (const balanceType of value.items) {
                this._balanceTypesMap.set(balanceType.id, balanceType);
            }
        }
    }

    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly localStorageService: LocalStorageService,
        private readonly siteSettingsService: SiteSettingsService,
        private readonly localizationService: LocalizationService,
        private readonly graphQlLocalizationsService: GraphQlLocalizationsService,
        private readonly graphQlDictionariesService: GraphQlDictionariesService,
        private readonly loaderService: LoaderService
    ) {
        this._dictionaries = this.localStorageService.getItem('dictionaries') as Dictionary || new Dictionary();
    }

    public initializePublic(): void {
        if (!this.siteSettingsService.version ||
            this.siteSettingsService.version.locale !== this.dictionaries?.locales?.version
        ) {
            this.loaderService.isBusy = true;
            forkJoin({
                result_locales: !this.siteSettingsService.version ||
                    this.siteSettingsService.version.country !== this.dictionaries?.countries?.version ?
                    this.graphQlLocalizationsService.getDictionaryLocales(
                    this.dictionaries?.locales?.version).pipe(take(1)) : of(undefined),
            }).pipe(
                tap(({ result_locales }) => {
                    if (this._dictionaries) {
                        if (!!result_locales) {
                            const locales = result_locales?.data?.localizations_get_locales_dictionary as VersionedListResponseOfLocaleResponse;
                            this._dictionaries.locales = locales;
                            this.localesMap = locales;
                        }

                        this.updateDateItems(true);
                        this.localStorageService.setItem('dictionaries', this.dictionaries);
                        this.loaderService.isBusy = false;
                    }
                }),
                handleApiError(this.snackBar)
            ).subscribe();
        } else {
            this.updateDateItems(true);
        }
    }

    public initializeNonPublic(): void {
        if (!this.siteSettingsService.version ||
            this.siteSettingsService.version.country !== this.dictionaries?.countries?.version ||
            this.siteSettingsService.version.currency !== this.dictionaries?.currencies?.version ||
            this.siteSettingsService.version.category !== this.dictionaries?.categories?.version ||
            this.siteSettingsService.version.frequency !== this.dictionaries?.frequencies?.version ||
            this.siteSettingsService.version.balanceType !== this.dictionaries?.balanceTypes?.version ||
            this.siteSettingsService.version.iconCategory !== this.dictionaries?.iconCategories?.version
        ) {
            this.loaderService.isBusy = true;
            forkJoin({
                result_countries: !this.siteSettingsService.version ||
                this.siteSettingsService.version.country !== this.dictionaries?.countries?.version ?
                    this.graphQlDictionariesService.getCountriesDictionary(
                    this.dictionaries?.countries?.version).pipe(take(1)) : of(undefined),
                result_currencies: !this.siteSettingsService.version ||
                this.siteSettingsService.version.currency !== this.dictionaries?.currencies?.version ?
                    this.graphQlDictionariesService.getCurrenciesDictionary(
                    this.dictionaries?.currencies?.version).pipe(take(1)) : of(undefined),
                result_categories: !this.siteSettingsService.version ||
                this.siteSettingsService.version.category !== this.dictionaries?.categories?.version ?
                    this.graphQlDictionariesService.getCategoriesDictionary(
                    this.dictionaries?.categories?.version).pipe(take(1)) : of(undefined),
                result_frequencies: !this.siteSettingsService.version ||
                this.siteSettingsService.version.frequency !== this.dictionaries?.frequencies?.version ?
                    this.graphQlDictionariesService.getFrequenciesDictionary(
                    this.dictionaries?.frequencies?.version).pipe(take(1)) : of(undefined),
                result_balance_types: !this.siteSettingsService.version ||
                this.siteSettingsService.version.balanceType !== this.dictionaries?.balanceTypes?.version ?
                    this.graphQlDictionariesService.getBalanceTypesDictionary(
                    this.dictionaries?.frequencies?.version).pipe(take(1)) : of(undefined),
                result_icon_categories: !this.siteSettingsService.version ||
                this.siteSettingsService.version.iconCategory !== this.dictionaries?.iconCategories?.version ?
                    this.graphQlDictionariesService.getIconCategoriesDictionary(
                    this.dictionaries?.iconCategories?.version).pipe(take(1)) : of(undefined),
            }).pipe(
                tap(({ result_countries, result_currencies, result_categories, result_frequencies, result_balance_types, result_icon_categories }) => {
                    if (this._dictionaries) {
                        if (!!result_countries) {
                            const countries = result_countries?.data?.dictionaries_get_countries_dictionary as VersionedListResponseOfCountryResponse;
                            this._dictionaries.countries = countries;
                            this.countriesMap = countries;
                        }

                        if (!!result_currencies) {
                            const currencies = result_currencies?.data?.dictionaries_get_currencies_dictionary as VersionedListResponseOfCurrencyResponse;
                            this._dictionaries.currencies = currencies;
                            this.currenciesMap = currencies;
                        }

                        if (!!result_categories) {
                            const categories = result_categories?.data?.dictionaries_get_categories_dictionary as VersionedListResponseOfCategoryResponse;
                            this._dictionaries.categories = categories;
                            this.categoriesMap = categories;
                        }

                        if (!!result_frequencies) {
                            const frequencies = result_frequencies?.data?.dictionaries_get_frequencies_dictionary as VersionedListResponseOfFrequencyResponse;
                            this._dictionaries.frequencies = frequencies;
                            this.frequenciesMap = frequencies;
                        }

                        if (!!result_balance_types) {
                            const balanceTypes = result_balance_types?.data?.dictionaries_get_balance_types_dictionary as VersionedListResponseOfBalanceTypeResponse;
                            this._dictionaries.balanceTypes = balanceTypes;
                            this.balanceTypesMap = balanceTypes;
                        }

                        if (!!result_icon_categories) {
                            const iconCategories = result_icon_categories?.data?.dictionaries_get_icon_categories_dictionary as VersionedListResponseOfIconCategoryResponse;
                            this._dictionaries.iconCategories = iconCategories;
                            this.iconCategoryMap = iconCategories;
                        }

                        this.updateDateItems(false);
                        this.localStorageService.setItem('dictionaries', this.dictionaries);
                        this.loaderService.isBusy = false;
                    }
                }),
                handleApiError(this.snackBar)
            ).subscribe();
        } else {
            this.updateDateItems(false);
        }
    }


    private updateDateItems(isPublic: boolean): void {
        if (this._dataItems === undefined) {
            this._dataItems = new DictionaryDataItems();
        }

        if (isPublic) {
            const tempLocales = this.dictionaries?.locales?.items
                .map(locale => new DataItem(locale, String(locale.id), locale.title, locale.titleEn));
            this._dataItems.locales = tempLocales?.sort((a, b) => Number(a.id) - Number(b.id));
        } else {
            this._dataItems.categories = this.mapCategories(this.dictionaries?.categories?.items || []);
            const tempCountries = this.dictionaries?.countries?.items
                .map(country => new DataItem(country, String(country.id), country.titleEn, country.title));
            this._dataItems.countries = tempCountries?.sort((a, b) => Number(a.id) - Number(b.id));
            const tempCurrencies = this.dictionaries?.currencies?.items
                .map(currency =>
                    new DataItem(
                        currency,
                        String(currency.id),
                        currency.titleEn,
                        `${currency.code} - ${currency.title}`,
                        undefined,
                        undefined,
                        [currency.titleEn, currency.code, currency.title],
                        true,
                        this._importantCurrencies.includes(currency.code)
                    )
                );
            this._dataItems.currencies = tempCurrencies?.sort((a, b) => {
                const importantCurrencies = this._importantCurrencies;
                const aIsImportant = importantCurrencies.includes((a.originalValue as CurrencyResponse).code);
                const bIsImportant = importantCurrencies.includes((b.originalValue as CurrencyResponse).code);

                if (aIsImportant && !bIsImportant) return -1;
                if (!aIsImportant && bIsImportant) return 1;
                return Number(a.id) - Number(b.id);
            });

            const tempFrequencies = this.dictionaries?.frequencies?.items
                .map(frequency => new DataItem(frequency, String(frequency.id), frequency.title, frequency.description));
            this._dataItems.frequencies = tempFrequencies?.sort((a, b) => Number(a.id) - Number(b.id));

            this._dataItems!.categoriesFlat = [];

            this.categoriesMap?.items.forEach(category => {
                this._dataItems!.categoriesFlat!.push(
                    new DataItem(
                        category,
                        String(category.id),
                        category.title,
                        category.isPositive ? 'EXPENSES.INCOME' : 'EXPENSES.EXPENSE',
                        category.iconId,
                        category.color,
                        this.localizationService.getAllTranslationsByKey(category.title) || []));
            });

            const tempBalanceTypes = this.dictionaries?.balanceTypes?.items
                .map(balance => new DataItem(balance, String(balance.id), balance.title, ''));
            this._dataItems.balanceTypes = tempBalanceTypes?.sort((a, b) => Number(a.id) - Number(b.id));
        }
    }

    private mapCategories(categories: CategoryResponse[]): DataItem[] {
        return categories.map(category => this.mapCategory(category));
    }

    private mapCategory(category: CategoryResponse): DataItem {
        const dataItem: DataItem = {
            originalValue: category,
            id: category?.id?.toString(),
            name: category?.title,
            description: category?.isPositive ? 'EXPENSES.INCOME' : 'EXPENSES.EXPENSE',
            iconId: category?.iconId,
            color: category?.color,
            isActive: category?.isActive,
            isImportant: false,
            children: category?.children ? this.mapCategories(category.children) : []
        };
        return dataItem;
    }

    private mapCategoriesToFlat(categories: CategoryResponse[]): CategoryResponse[] {
        let categoriesArr: CategoryResponse[] = [];

        categories.forEach(category => {
            this.mapCategoryToFlat(category!, categoriesArr);
        });

        return categoriesArr;
    }

    private mapCategoryToFlat(category: CategoryResponse, categories: CategoryResponse[]): CategoryResponse[] {
        if (!category) {
            return categories;
        }

        categories.push(category);

        if (category.children) {
            category.children.forEach(child => {
                this.mapCategoryToFlat(child!, categories);
            });
        }

        return categories;
    }
}