import {Injectable} from "@angular/core";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "./localization.service";
import {SiteSettingsService} from "./site-settings.service";
import {DictionaryService} from "./dictionary.service";
import {AuthService} from "./auth.service";
import {tap} from "rxjs";
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
        private readonly siteSettingsService: SiteSettingsService
    ) {}

    public initialize(): void {
        this.siteSettingsService.initialize();
        this.localizationService.initialize();

        this.authService.isAuthorized$
            ?.pipe(
                tap(isAuthorized => {
                    this.dictionaryService.initialize(isAuthorized);
                }),
                handleApiError(this.snackBar, this.localizationService)
            ).subscribe();
    }
}