import { Injectable } from '@angular/core';
import { LocalStorageService } from './local-storage.service';
import { BaseHttpService } from './base-http.service';
import {take, tap, switchMap, BehaviorSubject} from 'rxjs';
import { handleApiError } from '../helpers/rxjs.helper';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Dictionary } from '../models/common/dictionarie.model';
import { LocalizationData } from '../models/common/dictionary.model';
import {ISiteSettings, SiteSettings} from "../models/common/site-settings.model";
import {API_ROUTES} from "../helpers/api-route.helper";
import {environment} from "../environments/environment";
import {LocaleResponse} from "../models/localizations/localizations.model";

@Injectable({
  providedIn: 'root'
})
export class DictionaryService {
  private _dictionaries: Dictionary | undefined;
  private _localizations: LocalizationData | undefined;
  private _currentLocale: LocaleResponse | undefined;
  private _settings: SiteSettings | undefined;

  public localeChangedSub: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

  get dictionaries(): Dictionary | undefined {
    return this._dictionaries;
  }

  get localizations(): LocalizationData | undefined {
    return this._localizations;
  }

  get currentLocale(): LocaleResponse | undefined {
    return this._currentLocale;
  }

  get settings(): SiteSettings | undefined {
    return this._settings;
  }

  constructor(
    private readonly snackBar: MatSnackBar,
    private readonly baseHttpService: BaseHttpService,
    private readonly localStorageService: LocalStorageService,
  ) {}

  public initialize(): void {
    const settingsLocalStorage: SiteSettings | undefined = this.localStorageService.getItem('settings');

    this.baseHttpService.get<ISiteSettings | undefined>(API_ROUTES.DATA_API.SITE_SETTINGS.GET)
      .pipe(
        take(1),
        tap((data) => {
          this._settings = new SiteSettings(data);
          this._settings.clientVersion = environment.buildVersion;

          if (!this._settings) {
            this._settings = new SiteSettings({locale: 'en', apiVersion: 'honk', clientVersion: environment.buildVersion});
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

  public updateLocalizations(data: LocalizationData): void {
    this._localizations = data;
    this.localStorageService.setItem('localizations', data);
  }

  private initializeFromApi(): void {
    this.baseHttpService.get<LocalizationData>(API_ROUTES.DATA_API.LOCALIZATIONS.GET)
      .pipe(
        take(1),
        tap((data) => {
          this.updateLocalizations(data);
        }),
        switchMap(() =>
          this.baseHttpService.get<LocaleResponse[] | undefined>(API_ROUTES.DATA_API.LOCALIZATIONS.LOCALES)
            .pipe(
              take(1),
              tap((data) => {
                this._dictionaries = new Dictionary(data);
                this.localStorageService.setItem('dictionaries', this._dictionaries);
                this.setCurrentLocale();
              })
            )
        ),
        handleApiError(this.snackBar)
      ).subscribe();
  }

  private initializeFromLocalStorage(): void {
    const dictionaries: Dictionary | undefined = this.localStorageService.getItem('dictionaries');
    const localizations: LocalizationData | undefined = this.localStorageService.getItem('localizations');

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
