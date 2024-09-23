import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import {take, tap, switchMap, BehaviorSubject} from 'rxjs';
import { handleApiError } from '../helpers/rxjs.helper';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Dictionary } from '../models/common/dictionarie.model';
import {environment} from "../environments/environment";
import {LocaleResponse, LocalizationClient, LocalizationsResponse} from "../api-clients/localizations-client";
import {AuthClient, SiteSettingsResponse} from "../api-clients/auth-client";

@Injectable({
  providedIn: 'root'
})
export class DictionaryService {
  private _dictionaries: Dictionary | undefined;
  private _localizations: LocalizationsResponse | undefined;
  private _currentLocale: LocaleResponse | undefined;
  private _settings: SiteSettingsResponse | undefined;

  public localeChangedSub: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  get dictionaries(): Dictionary | undefined {
    return this._dictionaries;
  }

  get localizations(): LocalizationsResponse | undefined {
    return this._localizations;
  }

  get currentLocale(): LocaleResponse | undefined {
    return this._currentLocale;
  }

  get settings(): SiteSettingsResponse | undefined {
    return this._settings;
  }

  constructor(
    private readonly snackBar: MatSnackBar,
    private readonly authClient: AuthClient,
    private readonly localizationClient: LocalizationClient,
    private readonly localStorageService: LocalStorageService,
  ) {}

  public initialize(): void {
    const settingsLocalStorage: SiteSettingsResponse | undefined = this.localStorageService.getItem('settings');

    this.authClient.user_GetSettings()
      .pipe(
        take(1),
        tap((data) => {
          this._settings = data
          this._settings.clientVersion = environment.buildVersion;

          if (!this._settings) {
            this._settings = new SiteSettingsResponse({locale: 'en', apiVersion: 'honk', clientVersion: environment.buildVersion});
          }

          if (this._settings.locale != settingsLocalStorage?.locale) {
            this._settings.locale = settingsLocalStorage?.locale ?? 'en';
          }

          if (this._settings?.apiVersion !== settingsLocalStorage?.apiVersion ||
            this._settings?.clientVersion !== settingsLocalStorage?.clientVersion) {
            this.localStorageService.setItem('settings', this._settings);
            this.initializeFromApi();
          } else {
            this.initializeFromLocalStorage();
          }
        }),
        handleApiError(this.snackBar),
      ).subscribe();
  }

  public getTranslation(key: string | undefined): string | undefined {
    if (!key) {
      return undefined;
    }
    return !!this._localizations?.data ? (this._localizations.data[this._settings?.locale ?? 'en']?.[key] ?? key) : key;
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

  public localeChanged(id: number | undefined): void {
    this._currentLocale = this._dictionaries?.localeResponses?.find(l => l.id === id);
    if (this._settings) {
      this._settings.locale = this._currentLocale?.isoCode ?? 'en';
      this.localStorageService.setItem('settings', this._settings);
    }

    this.localeChangedSub.next(true);
  }

  public updateLocalizations(data: LocalizationsResponse): void {
    this._localizations = data;
    this.localStorageService.setItem('localizations', data);
  }

  private initializeFromApi(): void {
    this.localizationClient.localization_GetLocalization()
        .pipe(
            take(1),
            switchMap((data) =>{
              this.updateLocalizations(data);
              return this.localizationClient.localization_GetLocales()
            }),
            tap((data) => {
              this._dictionaries = new Dictionary(data);
              this.localStorageService.setItem('dictionaries', this._dictionaries);
              this.setCurrentLocale();
            }),
            handleApiError(this.snackBar)
        ).subscribe();
  }

  private initializeFromLocalStorage(): void {
    const dictionaries: Dictionary | undefined = this.localStorageService.getItem('dictionaries');
    const localizations: LocalizationsResponse | undefined = this.localStorageService.getItem('localizations');

    if (
      !dictionaries ||
      !localizations ||
      !dictionaries.localeResponses ||
      !localizations.data ||
      (!localizations.data || Object.keys(localizations.data).length === 0)
      ) {
      this.initializeFromApi();
      return;
    }

    this._dictionaries = dictionaries;
    this._localizations = localizations;

    this.setCurrentLocale();
  }

  private setCurrentLocale(): void {
    const locale = this._settings?.locale ?? 'en';
    this._currentLocale = this._dictionaries?.localeResponses?.find(l => l.isoCode === locale);

    this.localeChangedSub.next(true);
  }
}
