import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { take, tap, BehaviorSubject } from 'rxjs';
import { handleApiError } from '../helpers/rxjs.helper';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SiteSettingsService } from './site-settings.service';
import { LocalizationsResponse, SiteSettingsResponse, LocalizationResponse, LocalizationItemResponse } from '../api-clients/common-module.client';
import { GraphQlLocalizationsService } from '../graph-ql/services/graph-ql-localizations.service';

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

        const mergedData: { [key: string]: LocalizationItemResponse[] } = {};

        if (this._nonPublicLocalizations?.data) {
            this._nonPublicLocalizations.data.forEach(locale => {
                mergedData[locale.locale] = locale.items;
            });
        }

        if (this._publicLocalizations?.data) {
            this._publicLocalizations.data.forEach(locale => {
                if (!mergedData[locale.locale]) {
                    mergedData[locale.locale] = [];
                }
                locale.items.forEach(item => {
                    const existingItem = mergedData[locale.locale].find(i => i.key === item.key);
                    if (existingItem) {
                        existingItem.value = item.value;
                    } else {
                        mergedData[locale.locale].push(item);
                    }
                });
            });
        }

        const mergedLocalizations = Object.keys(mergedData).map(locale => new LocalizationResponse({
            locale,
            items: mergedData[locale]
        }));

        return new LocalizationsResponse({
            version: this._nonPublicLocalizations?.version ?? this._publicLocalizations?.version,
            data: mergedLocalizations
        });
    }

    get currentLocaleCode(): string {
        return this.siteSettingsService.siteSettings?.locale ?? 'en';
    }

    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly localStorageService: LocalStorageService,
        private readonly siteSettingsService: SiteSettingsService,
        private readonly graphQlLocalizationsService: GraphQlLocalizationsService
    ) { }

    public initialize(isPublic: boolean): void {
        if (!this.siteSettingsService.version ||
            (isPublic && this.siteSettingsService.version.localizationPublic !== this.publicLocalizations?.version) ||
            (!isPublic && this.siteSettingsService.version.localization !== this.nonPublicLocalizations?.version)) {
            if (isPublic) {
                this.graphQlLocalizationsService.getPublicLocalizations(this.publicLocalizations?.version)
                    .pipe(
                        take(1),
                        tap((result) => {
                            const data = result?.data?.localizations_get_public_localizations as LocalizationsResponse;
                            if (data && data.data.length > 0) {
                                this.publicLocalizations = data;
                            }
                            this.localeChangedSub.next(true);
                        }),
                        handleApiError(this.snackBar)
                    ).subscribe();
            } else {
                this.graphQlLocalizationsService.getLocalizations(this.nonPublicLocalizations?.version)
                    .pipe(
                        take(1),
                        tap((result) => {
                            const data = result?.data?.localizations_get_localizations as LocalizationsResponse;
                            if (data && data.data.length > 0) {
                                this.nonPublicLocalizations = data;
                            }
                            this.localeChangedSub.next(true);
                        }),
                        handleApiError(this.snackBar)
                    ).subscribe();
            }
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

        const publicTranslation = this._publicLocalizations?.data.find(l => l.locale === locale)?.items.find(i => i.key === key)?.value;
        const nonPublicTranslation = this._nonPublicLocalizations?.data.find(l => l.locale === locale)?.items.find(i => i.key === key)?.value;

        return publicTranslation ?? nonPublicTranslation ?? key;
    }

    public getAllTranslations(locale: string): { [key: string]: string } | undefined {
        const translations: { [key: string]: string } = {};

        this._publicLocalizations?.data.find(l => l.locale === locale)?.items.forEach(item => {
            translations[item.key] = item.value;
        });

        this._nonPublicLocalizations?.data.find(l => l.locale === locale)?.items.forEach(item => {
            translations[item.key] = item.value;
        });

        return translations;
    }

    public getAllTranslationsByKey(key: string): string[] | undefined {
        const translations: Set<string> = new Set();

        this._publicLocalizations?.data.forEach(locale => {
            const translation = locale.items.find(i => i.key === key)?.value;
            if (translation) {
                translations.add(translation);
            }
        });

        this._nonPublicLocalizations?.data.forEach(locale => {
            const translation = locale.items.find(i => i.key === key)?.value;
            if (translation) {
                translations.add(translation);
            }
        });

        return Array.from(translations);
    }

    public localeChanged(code: string | undefined): void {
        const currentSettings = this.siteSettingsService.siteSettings;
        if (currentSettings) {
            this.siteSettingsService.siteSettings = {
                ...currentSettings,
                locale: code ?? 'en'
            } as SiteSettingsResponse;

            this.localeChangedSub.next(true);
        }
    }
}