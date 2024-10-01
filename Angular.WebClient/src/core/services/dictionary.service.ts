import { Injectable } from "@angular/core";
import {
    DictionaryClient,
    GetCategoriesRequest,
    GetCountriesRequest,
    GetCurrenciesRequest,
    TreeNodeResponseOfCategoryResponse
} from "../api-clients/dictionaries-client";
import {catchError, forkJoin, take, tap} from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { handleApiError } from "../helpers/rxjs.helper";
import {
    GetLocalesRequest,
    LocaleResponse,
    LocalizationClient
} from "../api-clients/localizations-client";
import {Dictionary, DictionaryDataItems} from "../models/common/dictionarie.model";
import {LocalStorageService} from "./local-storage.service";
import {SiteSettingsService} from "./site-settings.service";
import {DataItem} from "../models/common/data-item.model";

@Injectable({
    providedIn: 'root'
})
export class DictionaryService {
    _dictionaries: Dictionary | undefined;

    get dictionaries(): Dictionary | undefined {
        return this._dictionaries;
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
        private readonly siteSettingsService: SiteSettingsService
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
                        this.updateDateItems(true);
                        this.localStorageService.setItem('dictionaries', this.dictionaries);
                    }
                }),
                catchError(handleApiError(this.snackBar))
            ).subscribe();
        }
    }

    public initializeNonPublic(): void {
        if (!this.siteSettingsService.version ||
            this.siteSettingsService.version.country !== this.dictionaries?.countries?.version ||
            this.siteSettingsService.version.currency !== this.dictionaries?.currencies?.version ||
            this.siteSettingsService.version.category !== this.dictionaries?.categories?.version
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
                    })).pipe(take(1))
            }).pipe(
                tap(({ countries, currencies, categories }) => {
                    if (this._dictionaries) {
                        this._dictionaries.countries = countries;
                        this._dictionaries.currencies = currencies;
                        this._dictionaries.categories = categories;

                        this.updateDateItems(false);
                        this.localStorageService.setItem('dictionaries', this.dictionaries);
                    }
                }),
                catchError(handleApiError(this.snackBar))
            ).subscribe();
        }
    }


    private updateDateItems(isPublic: boolean): void {
        if (this.dictionaries?.dataItems === undefined) {
            this.dictionaries!.dataItems = new DictionaryDataItems();
        }

        if (isPublic) {
            this.dictionaries!.dataItems.locales = this.dictionaries!.locales?.items.map(locale => new DataItem(String(locale.id), locale.title, locale.titleEn));
        } else {
            this.dictionaries!.dataItems.countries = this.dictionaries!.countries?.items.map(country => new DataItem(String(country.id), country.titleEn, country.title));
            this.dictionaries!.dataItems.currencies = this.dictionaries!.currencies?.items.map(currency => new DataItem(String(currency.id), currency.titleEn, `${currency.code} - ${currency.title}`));
            this.dictionaries!.dataItems.categories = this.mapCategories(this.dictionaries!.categories?.items || []);
        }
    }

    private mapCategories(categories: TreeNodeResponseOfCategoryResponse[]): DataItem[] {
        return categories.map(category => this.mapCategory(category));
    }

    private mapCategory(category: TreeNodeResponseOfCategoryResponse): DataItem {
        const dataItem: DataItem = {
            id: category.node?.id?.toString(),
            name: category.node?.title,
            description: category.node?.icon,
            isActive: category.node?.isActive,
            isImportant: category.node?.isPositive,
            children: category.node?.children ? this.mapCategories(category.node.children) : []
        };
        return dataItem;
    }
}