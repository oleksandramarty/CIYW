import {Injectable} from '@angular/core';
import {LocalStorageService} from './local-storage.service';
import {take, tap, switchMap, BehaviorSubject} from 'rxjs';
import {handleApiError} from '../helpers/rxjs.helper';
import {MatSnackBar} from '@angular/material/snack-bar';
import {LocaleResponse, LocalizationClient, LocalizationsResponse} from "../api-clients/localizations-client";
import {SiteSettingsResponse} from "../api-clients/dictionaries-client";
import {SiteSettingsService} from "./site-settings.service";
import {environment} from "../environments/environment";

@Injectable({
    providedIn: 'root'
})
export class LocalizationService {
    private _localizations: LocalizationsResponse | undefined;

    public localeChangedSub: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    get localizations(): LocalizationsResponse | undefined {
        if (!this._localizations) {
            this._localizations = this.localStorageService.getItem('localizations');
        }
        return this._localizations;
    }

    set localizations(value: LocalizationsResponse | undefined) {
        this._localizations = value;
        this.localStorageService.setItem('localizations', this._localizations);
    }

    //TODO fix it
    get currentLocaleCode(): string {
        return 'en';
    }

    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly localizationClient: LocalizationClient,
        private readonly localStorageService: LocalStorageService,
        private readonly siteSettingsService: SiteSettingsService
    ) {
    }

    public initialize(): void {
        if (!this.localizations || this.localizations?.version !== environment.buildVersion) {
            this.reinitialize();
        }
    }

    public reinitialize(): void {
        this.localizationClient.localization_GetLocalizations()
            .pipe(
                take(1),
                tap((data) => {
                    this.localizations = data;
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    public getTranslation(key: string | undefined): string | undefined {
        if (!key) {
            return undefined;
        }
        return !!this._localizations?.data ? (this._localizations.data[this.currentLocaleCode]?.[key] ?? key) : key;
    }

    public getTranslationByLocale(locale: string | undefined, key: string | undefined): string | undefined {
        if (!key) {
            return undefined;
        }
        return !!this._localizations?.data ? (this._localizations.data[locale ?? 'en']?.[key] ?? key) : key;
    }

    public getAllTranslations(locale: string): { [key: string]: string } | undefined {
        if (!this._localizations?.data) {
            return undefined;
        }

        return this._localizations.data[locale];
    }

    public localeChanged(code: string | undefined): void {
        const currentSettings = this.siteSettingsService.siteSettings;
        if (currentSettings) {
            this.siteSettingsService.siteSettings =
                new SiteSettingsResponse({
                    ...currentSettings,
                    locale: code ?? 'en'
                });

            this.localeChangedSub.next(true);
        }
    }
}
