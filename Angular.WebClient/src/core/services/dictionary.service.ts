import { Injectable } from "@angular/core";
import {
    DictionaryClient,
    GetCategoriesRequest,
    GetCountriesRequest,
    GetCurrenciesRequest
} from "../api-clients/dictionaries-client";
import {catchError, forkJoin, Observable, of, take, tap} from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { handleApiError } from "../helpers/rxjs.helper";
import {LocaleResponse, LocalizationClient} from "../api-clients/localizations-client";
import { Dictionary } from "../models/common/dictionarie.model";
import {LocalStorageService} from "./local-storage.service";
import {environment} from "../environments/environment";
import {SiteSettingsService} from "./site-settings.service";

@Injectable({
    providedIn: 'root'
})
export class DictionaryService {
    private _dictionaries: Dictionary | undefined;

    get dictionaries(): Dictionary | undefined {
        if (!this._dictionaries) {
            this._dictionaries = this.localStorageService.getItem('dictionaries');
        }
        return this._dictionaries;
    }

    set dictionaries(value: Dictionary | undefined) {
        this._dictionaries = value;
        this.localStorageService.setItem('dictionaries', this._dictionaries);
    }

    get currentLocale(): LocaleResponse | undefined {
        return this.dictionaries?.
        locales?.
        find(locale => locale.isoCode === this.siteSettingsService?.siteSettings?.locale);
    }

    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly localizationClient: LocalizationClient,
        private readonly dictionaryClient: DictionaryClient,
        private readonly localStorageService: LocalStorageService,
        private readonly siteSettingsService: SiteSettingsService
    ) {
        this.dictionaries = new Dictionary(localStorageService);
    }

    public initialize(isAuthorized: boolean): void {
        if (!this.dictionaries || !this.dictionaries.isLoaded()) {
            this.reinitialize(isAuthorized);
        }
    }

    public reinitialize(isAuthorized: boolean): void {
        forkJoin({
            countries: isAuthorized ?
                this.dictionaryClient.dictionary_GetCountries(
                    new GetCountriesRequest({
                        version: this.dictionaries?.countriesVersion,
                        count: this.dictionaries?.countriesCount
                    })).pipe(take(1)) :
                of(undefined),
            currencies: isAuthorized ?
                    this.dictionaryClient.dictionary_GetCurrencies(
                        new GetCurrenciesRequest({
                            version: this.dictionaries?.currenciesVersion,
                            count: this.dictionaries?.currenciesCount
                        })).pipe(take(1)) :
                    of(undefined),
            categories: isAuthorized ?
                this.dictionaryClient.dictionary_GetCategories(
                    new GetCategoriesRequest({
                            version: this.dictionaries?.categoriesVersion,
                            count: this.dictionaries?.categoriesCount
                        })).pipe(take(1)) :
                of(undefined),
            locales: !this.dictionaries?.currencies ?
                this.localizationClient.localization_GetLocales().pipe(take(1)) :
                of(undefined)
        }).pipe(
            tap(({ countries, currencies, categories, locales }) => {
                if (this.dictionaries) {
                    this.dictionaries.countries = countries;
                    this.dictionaries.currencies = currencies;
                    this.dictionaries.categories = categories;
                    this.dictionaries.locales = locales;

                    this.dictionaries.updateDateItems();
                }
            }),
            catchError(handleApiError(this.snackBar))
        ).subscribe();
    }
}