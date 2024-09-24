import { Injectable } from "@angular/core";
import { DictionaryClient } from "../api-clients/dictionaries-client";
import {catchError, forkJoin, Observable, of, take, tap} from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { handleApiError } from "../helpers/rxjs.helper";
import {LocaleResponse, LocalizationClient} from "../api-clients/localizations-client";
import { Dictionary } from "../models/common/dictionarie.model";
import {LocalStorageService} from "./local-storage.service";
import {environment} from "../environments/environment";

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
        find(locale => locale.isoCode === this.dictionaries?.siteSettings?.locale);
    }

    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly localizationClient: LocalizationClient,
        private readonly dictionaryClient: DictionaryClient,
        private readonly localStorageService: LocalStorageService
    ) {
        this.dictionaries = new Dictionary();
    }

    public initialize(isAuthorized: boolean): void {
        if (!this.dictionaries || this.dictionaries.isLoaded()) {
            this.reinitialize(isAuthorized);
        }
    }

    public reinitialize(isAuthorized: boolean): void {
        forkJoin({
            countries: isAuthorized && !this.dictionaries?.countries ?
                this.dictionaryClient.dictionary_GetCountries().pipe(take(1)) :
                of(undefined),
            currencies: isAuthorized && !this.dictionaries?.currencies ?
                    this.dictionaryClient.dictionary_GetCurrencies().pipe(take(1)) :
                    of(undefined),
            categories: isAuthorized && !this.dictionaries?.currencies ?
                this.dictionaryClient.dictionary_GetCategories().pipe(take(1)) :
                of(undefined),
            locales: !this.dictionaries?.currencies ?
                this.localizationClient.localization_GetLocales().pipe(take(1)) :
                of(undefined),
            siteSettings: !this.dictionaries?.currencies ?
                this.dictionaryClient.siteSetting_GetSettings().pipe(take(1)) :
                of(undefined)
        }).pipe(
            tap(({ countries, currencies, categories, locales, siteSettings }) => {
                if (this.dictionaries) {
                    this.dictionaries.countries = countries;
                    this.dictionaries.currencies = currencies;
                    this.dictionaries.categories = categories;
                    this.dictionaries.locales = locales;
                    this.dictionaries.siteSettings = siteSettings;

                    this.dictionaries.updateDateItems();
                }
            }),
            catchError(handleApiError(this.snackBar))
        ).subscribe();
    }
}