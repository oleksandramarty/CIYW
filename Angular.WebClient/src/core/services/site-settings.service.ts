import {Injectable} from "@angular/core";
import {LocalStorageService} from "./local-storage.service";
import {CacheVersionResponse, SiteSettingsResponse} from "../api-models/common.models";

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

    get version(): CacheVersionResponse | undefined {
        return this.siteSettings?.version;
    }

    set siteSettings(value: SiteSettingsResponse | undefined) {
        this._siteSettings = value;
        this.localStorageService.setItem('settings', this._siteSettings);
    }

    constructor(
        private readonly localStorageService: LocalStorageService
    ) {
    }
}