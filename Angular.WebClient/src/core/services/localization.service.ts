import {Injectable} from '@angular/core';
import {LocalStorageService} from './local-storage.service';
import {take, tap, BehaviorSubject} from 'rxjs';
import {handleApiError} from '../helpers/rxjs.helper';
import {MatSnackBar} from '@angular/material/snack-bar';
import {
    GetLocalizationsRequest,
    LocalizationClient,
    LocalizationsResponse
} from "../api-clients/localizations-client";
import {SiteSettingsService} from "./site-settings.service";
import {SiteSettingsResponse} from "../api-clients/common-module.client";

@Injectable({
    providedIn: 'root'
})
export class LocalizationService {
    private _nonPublicLocalizations: LocalizationsResponse | undefined;
    private _publicLocalizations: LocalizationsResponse | undefined;

    public localeChangedSub: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

    get nonPublicLocalizations(): LocalizationsResponse | undefined {
        if (!this._nonPublicLocalizations) {
            this._nonPublicLocalizations = this.localStorageService.getItem('localization');
        }
        return this._nonPublicLocalizations;
    }

    set nonPublicLocalizations(value: LocalizationsResponse | undefined) {
        this._nonPublicLocalizations = value;
        this.localStorageService.setItem('localization', value);
    }

    get publicLocalizations(): LocalizationsResponse | undefined {
        if (!this._publicLocalizations) {
            this._publicLocalizations = this.localStorageService.getItem('localization_public');
        }
        return this._publicLocalizations;
    }

    set publicLocalizations(value: LocalizationsResponse | undefined) {
        this._publicLocalizations = value;
        this.localStorageService.setItem('localization_public', value);
    }

    get getAllLocalizations(): LocalizationsResponse | undefined {
        if (!this._nonPublicLocalizations && !this._publicLocalizations) {
            return undefined;
        }

        const mergedData: { [key: string]: { [key: string]: string } } = {};

        if (this._nonPublicLocalizations?.data) {
            Object.assign(mergedData, this._nonPublicLocalizations.data);
        }

        if (this._publicLocalizations?.data) {
            for (const locale in this._publicLocalizations.data) {
                if (this._publicLocalizations.data.hasOwnProperty(locale)) {
                    mergedData[locale] = {
                        ...mergedData[locale],
                        ...this._publicLocalizations.data[locale]
                    };
                }
            }
        }

        return new LocalizationsResponse({
            version: this._nonPublicLocalizations?.version ?? this._publicLocalizations?.version,
            data: mergedData
        });
    }

    get currentLocaleCode(): string {
        return this.siteSettingsService.siteSettings?.locale ?? 'en';
    }

    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly localizationClient: LocalizationClient,
        private readonly localStorageService: LocalStorageService,
        private readonly siteSettingsService: SiteSettingsService
    ) {
    }

    public initialize(isPublic: boolean): void {
        if (!this.siteSettingsService.version ||
            isPublic && this.siteSettingsService.version.localizationPublic !== this.publicLocalizations?.version ||
            !isPublic && this.siteSettingsService.version.localization !== this.nonPublicLocalizations?.version) {
            (isPublic ?
                this.localizationClient.localization_GetPublicLocalizations(
                    new GetLocalizationsRequest(
                        {
                            version: this.publicLocalizations?.version,
                            isPublic: true
                        })) :
                this.localizationClient.localization_GetLocalizations(
                    new GetLocalizationsRequest(
                        {
                            version: this.nonPublicLocalizations?.version,
                            isPublic: false
                        })))
                .pipe(
                    take(1),
                    tap((data) => {
                        if (!!data && Object.keys(data.data).length > 0) {
                            if (isPublic) {
                                this.publicLocalizations = data;
                            } else {
                                this.nonPublicLocalizations = data;
                            }
                        }
                        this.localeChangedSub.next(true);
                    }),
                    handleApiError(this.snackBar)
                ).subscribe();
        }

        this.localeChangedSub.next(true);
    }

    public getTranslation(key: string | undefined): string | undefined {
        return this.getTranslationByLocale(this.currentLocaleCode, key);
    }

    public getTranslationByLocale(locale: string | undefined, key: string | undefined): string | undefined {
        if (!key) {
            return undefined;
        }

        return this._publicLocalizations?.data[locale ?? 'en']?.[key] ??
            this._nonPublicLocalizations?.data[locale ?? 'en']?.[key] ?? key;
    }

    public getAllTranslations(locale: string): { [key: string]: string } | undefined {
        return this.getAllLocalizations?.data[locale];
    }

    public getAllTranslationsByKey(key: string): string[] | undefined {
        const translations: Set<string> = new Set();

        Object.keys(this.publicLocalizations?.data ?? {}).forEach((locale) => {
            const translation = this.publicLocalizations?.data[locale ?? 'en']?.[key];
            if (translation) {
                translations.add(translation);
            }
        });
        Object.keys(this.nonPublicLocalizations?.data ?? {}).forEach((locale) => {
            const translation = this.nonPublicLocalizations?.data[locale ?? 'en']?.[key];
            if (translation) {
                translations.add(translation);
            }
        });

        return Array.from(translations);
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
