import { Injectable } from "@angular/core";
import {
    CategoryResponse,
    CountryResponse,
    CurrencyResponse,
    DictionaryClient, FrequencyResponse,
    GetCategoriesRequest,
    GetCountriesRequest,
    GetCurrenciesRequest,
    TreeNodeResponseOfCategoryResponse,
    VersionedListOfCountryResponse, VersionedListOfCurrencyResponse, VersionedListOfFrequencyResponse,
    VersionedListOfTreeNodeResponseOfCategoryResponse
} from "../api-clients/dictionaries-client";
import {catchError, forkJoin, take, tap} from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { handleApiError } from "../helpers/rxjs.helper";
import {
    GetLocalesRequest,
    LocaleResponse,
    LocalizationClient, VersionedListOfLocaleResponse
} from "../api-clients/localizations-client";
import {Dictionary, DictionaryDataItems, DictionaryMap} from "../models/common/dictionarie.model";
import {LocalStorageService} from "./local-storage.service";
import {SiteSettingsService} from "./site-settings.service";
import {DataItem} from "../models/common/data-item.model";
import {LocalizationService} from "./localization.service";

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

    private readonly _importantCurrencies = ['USD', 'EUR', 'GBP', 'CAD', 'MDL', 'UAH']

    get dictionaries(): Dictionary | undefined {
        return this._dictionaries;
    }

    get dataItems(): DictionaryDataItems | undefined {
        return this._dataItems;
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

    set countriesMap(value: VersionedListOfCountryResponse | undefined) {
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

    set currenciesMap(value: VersionedListOfCurrencyResponse | undefined) {
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

    set categoriesMap(value: VersionedListOfTreeNodeResponseOfCategoryResponse | undefined) {
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

    set localesMap(value: VersionedListOfLocaleResponse | undefined) {
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

    set frequenciesMap(value: VersionedListOfFrequencyResponse | undefined) {
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

    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly localizationClient: LocalizationClient,
        private readonly dictionaryClient: DictionaryClient,
        private readonly localStorageService: LocalStorageService,
        private readonly siteSettingsService: SiteSettingsService,
        private readonly localizationService: LocalizationService
    ) {
        this._dictionaries = this.localStorageService.getItem('dictionaries') as Dictionary || new Dictionary();
    }

    public initializePublic(): void {
        if (!this.siteSettingsService.version ||
            this.siteSettingsService.version.locale !== this.dictionaries?.locales?.version
        ) {
            forkJoin({
                locales: this.localizationClient.localization_GetLocales(
                    new GetLocalesRequest({
                        version: this.dictionaries?.locales?.version
                    })
                ).pipe(take(1))
            }).pipe(
                tap(({ locales }) => {
                    if (this._dictionaries) {
                        this._dictionaries.locales = locales;

                        this.localesMap = locales;

                        this.updateDateItems(true);
                        this.localStorageService.setItem('dictionaries', this.dictionaries);
                    }
                }),
                catchError(handleApiError(this.snackBar))
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
            this.siteSettingsService.version.frequency !== this.dictionaries?.frequencies?.version
        ) {
            forkJoin({
                countries: this.dictionaryClient.dictionary_GetCountries(
                    new GetCountriesRequest({
                        version: this.dictionaries?.countries?.version
                    })).pipe(take(1)),
                currencies: this.dictionaryClient.dictionary_GetCurrencies(
                    new GetCurrenciesRequest({
                        version: this.dictionaries?.countries?.version
                    })).pipe(take(1)),
                categories: this.dictionaryClient.dictionary_GetCategories(
                    new GetCategoriesRequest({
                        version: this.dictionaries?.countries?.version
                    })).pipe(take(1)),
                frequencies: this.dictionaryClient.dictionary_GetFrequencies(
                    new GetCategoriesRequest({
                        version: this.dictionaries?.countries?.version
                    })).pipe(take(1))
            }).pipe(
                tap(({ countries, currencies, categories, frequencies }) => {
                    if (this._dictionaries) {
                        this._dictionaries.countries = countries;
                        this._dictionaries.currencies = currencies;
                        this._dictionaries.categories = categories;
                        this._dictionaries.frequencies = frequencies;

                        this.countriesMap = countries;
                        this.currenciesMap = currencies;
                        this.categoriesMap = categories;
                        this.frequenciesMap = frequencies;

                        this.updateDateItems(false);
                        this.localStorageService.setItem('dictionaries', this.dictionaries);
                    }
                }),
                catchError(handleApiError(this.snackBar))
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
            this._dataItems.locales = this._dictionaries?.locales?.items
                .sort((a, b) => a.id - b.id)
                .map(locale => new DataItem(locale, String(locale.id), locale.title, locale.titleEn));
        } else {
            this._dataItems.categories = this.mapCategories(this.dictionaries?.categories?.items || []);
            this._dataItems.countries = this.dictionaries?.countries?.items
                .sort((a, b) => a.id - b.id)
                .map(country => new DataItem(country, String(country.id), country.titleEn, country.title));
            this._dataItems.currencies = this.dictionaries?.currencies?.items
                .sort((a, b) => {
                    const importantCurrencies = this._importantCurrencies;
                    const aIsImportant = importantCurrencies.includes(a.code);
                    const bIsImportant = importantCurrencies.includes(b.code);

                    if (aIsImportant && !bIsImportant) return -1;
                    if (!aIsImportant && bIsImportant) return 1;
                    return a.id - b.id;
                })
                .map(currency =>
                    new DataItem(
                        currency,
                        String(currency.id),
                        currency.titleEn,
                        `${currency.code} - ${currency.title}`,
                        [currency.titleEn, currency.code, currency.title],
                        true,
                        this._importantCurrencies.includes(currency.code)
                    )
                );
            this._dataItems.frequencies = this.dictionaries?.frequencies?.items
                .sort((a, b) => a.id - b.id)
                .map(frequency => new DataItem(frequency, String(frequency.id), frequency.title, frequency.description));

            this._dataItems!.categoriesFlat = [];

            this.categoriesMap?.items.forEach(category => {
                this._dataItems!.categoriesFlat!.push(
                    new DataItem(
                        category,
                        String(category.id),
                        category.title,
                        category.icon,
                        this.localizationService.getAllTranslationsByKey(category.title) || []));
            });
        }
    }

    private mapCategories(categories: TreeNodeResponseOfCategoryResponse[]): DataItem[] {
        return categories.map(category => this.mapCategory(category));
    }

    private mapCategory(category: TreeNodeResponseOfCategoryResponse): DataItem {
        const dataItem: DataItem = {
            originalValue: category.node,
            id: category.node?.id?.toString(),
            name: category.node?.title,
            description: category.node?.icon,
            isActive: category.node?.isActive,
            isImportant: false,
            children: category.node?.children ? this.mapCategories(category.node.children) : []
        };
        return dataItem;
    }

    private mapCategoriesToFlat(categories: TreeNodeResponseOfCategoryResponse[]): CategoryResponse[] {
        let categoriesArr: CategoryResponse[] = [];

        categories.forEach(category => {
            this.mapCategoryToFlat(category.node!, categoriesArr);
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
                this.mapCategoryToFlat(child.node!, categories);
            });
        }

        return categories;
    }
}