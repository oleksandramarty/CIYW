import {Injectable} from "@angular/core";
import {DictionaryClient, SiteSettingsResponse} from "../api-clients/dictionaries-client";
import {LocalStorageService} from "./local-storage.service";
import {environment} from "../environments/environment";
import {take, tap} from "rxjs";
import {handleApiError} from "../helpers/rxjs.helper";
import {LocalizationClient} from "../api-clients/localizations-client";
import {DictionaryService} from "./dictionary.service";
import {MatSnackBar} from "@angular/material/snack-bar";

@Injectable({
    providedIn: "root"
})
export class SiteSettingsService {
    private _siteSettings: SiteSettingsResponse | undefined;

    get siteSettings(): SiteSettingsResponse | undefined {
        if (!this._siteSettings) {
            this._siteSettings = this.localStorageService.getItem('settings');
        }
        return this._siteSettings;
    }

    set siteSettings(value: SiteSettingsResponse | undefined) {
        this._siteSettings = value;
        this.localStorageService.setItem('settings', this._siteSettings);
    }

    constructor(
        private readonly localStorageService: LocalStorageService,
        private readonly dictionaryClient: DictionaryClient,
        private readonly snackBar: MatSnackBar
    ) {
    }

    public initialize(): void {
        if (!this.siteSettings || this.siteSettings?.buildVersion !== environment.buildVersion) {
            this.reinitialize();
        }
    }

    public reinitialize(): void {
        this.dictionaryClient.siteSetting_GetSettings()
            .pipe(
                take(1),
                tap((data) => {
                    this.siteSettings = data;
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }
}