import {Injectable} from "@angular/core";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "./localization.service";
import {SiteSettingsService} from "./site-settings.service";
import {DictionaryService} from "./dictionary.service";
import {filter, take, tap} from "rxjs";
import {GraphQlDictionariesService} from "../graph-ql/services/graph-ql-dictionaries.service";
import {SiteSettingsResponse} from "../api-models/common.models";
import {AuthService} from "./auth.service";
import {handleApiError} from "../helpers/rxjs.helper";

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
        private readonly graphQlDictionariesService: GraphQlDictionariesService
    ) {}

    public initialize(): void {
        this.graphQlDictionariesService.siteSettings()
            .pipe(
                take(1),
                tap((result) => {
                    this.siteSettingsService.siteSettings = result?.data?.dictionaries_site_settings as SiteSettingsResponse;
                    this.initializeCache();
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    private initializeCache(): void {
        this.localizationService.initialize(true);
        this.dictionaryService.initializePublic();
        this.authService.isAuthorized$
            ?.pipe(
                filter(isAuthorized => isAuthorized !== undefined),
                tap(isAuthorized => {
                    if (isAuthorized) {
                        this.dictionaryService.initializeNonPublic();
                        this.localizationService.initialize(false);
                    }
                }),
                handleApiError(this.snackBar, this.localizationService)
            ).subscribe();
    }
}