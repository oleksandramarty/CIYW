import {Injectable} from "@angular/core";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "./localization.service";
import {SiteSettingsService} from "./site-settings.service";
import {DictionaryService} from "./dictionary.service";
import {AuthService} from "./auth.service";
import {take, tap} from "rxjs";
import {handleApiError} from "../helpers/rxjs.helper";
import {DictionaryClient} from "../api-clients/dictionaries-client";

@Injectable({
    providedIn: "root"
})
export class BaseInitializationService {
    constructor(
        private readonly snackBar: MatSnackBar,
        private readonly authService: AuthService,
        private readonly dictionaryService: DictionaryService,
        private readonly localizationService: LocalizationService,
        private readonly siteSettingsService: SiteSettingsService,
        private readonly dictionaryClient: DictionaryClient,
    ) {}

    public initialize(): void {
        this.dictionaryClient.siteSetting_GetSettings()
            .pipe(
                take(1),
                tap((data) => {
                    this.siteSettingsService.siteSettings = data;

                    this.initializeCache();
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    private initializeCache(): void {
        this.authService.isAuthorized$
            ?.pipe(
                tap(isAuthorized => {
                    this.localizationService.initialize(true);
                    this.dictionaryService.initializePublic();
                    if (isAuthorized) {
                        this.dictionaryService.initializeNonPublic();
                        this.localizationService.initialize(false);
                    }
                }),
                handleApiError(this.snackBar, this.localizationService)
            ).subscribe();
    }
}