import {Component, OnInit} from '@angular/core';
import {Store} from '@ngrx/store';
import {Observable, takeUntil, tap} from 'rxjs';
import {LocalizationService} from '../../../../core/services/localization.service';
import {selectUser} from '../../../../core/store/selectors/auth.selectors';
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {AuthService} from "../../../../core/services/auth.service";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {LocaleResponse, UserResponse} from "../../../../core/api-models/common.models";
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {auth_setUser} from "../../../../core/store/actions/auth.actions";
import {selectMenuState} from "../../../../core/store/selectors/site.selectors";
import {menu_toggle} from "../../../../core/store/actions/site.actions";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent extends BaseAuthorizeComponent {
    public langFlags: Map<string, string> = new Map([
        ['en', '🇬🇧'], ['fr', '🇫🇷'], ['de', '🇩🇪'],
        ['ua', '🇺🇦'], ['ru', '🇷🇺'], ['es', '🇪🇸'],
        ['it', '🇮🇹'],
    ]);

    public isUserMenu: boolean = true;

    public menuItems: { url: string, title: string }[] = [
        {url: 'dashboard', title: 'MENU.DASHBOARD'},
        {url: 'projects', title: 'MENU.PROJECTS'},
        {url: 'analytics', title: 'MENU.ANALYTICS'},
    ];

    public menuAdminItems: { url: string, title: string }[] = [
        {url: 'admin/home', title: 'MENU.HOME'},
        {url: 'admin/audit-trail', title: 'MENU.AUDIT_TRAIL'},
    ];

    get locales(): LocaleResponse[] | undefined {
        return this.dictionaryService.dictionaries?.locales?.items ?? [];
    }

    get currentLocale(): LocaleResponse | undefined {
        return this.dictionaryService.currentLocale;
    }

    constructor(
        protected override readonly store: Store,
        protected override readonly snackBar: MatSnackBar,
        protected override readonly authService: AuthService,
        private router: Router,
        private readonly localizationService: LocalizationService,
        private readonly dictionaryService: DictionaryService,
    ) {
        super(authService, store, snackBar);
    }

    public goto(url: string | undefined): void {
        this.router.navigate([`/${url ?? ''}`]);
    }

    public localeChanged(code: string | undefined): void {
        this.localizationService.localeChanged(code);
    }

    public logout() {
        this.authService.logout()
    }

    public toggleMenu(): void {
        this.store.dispatch(menu_toggle());
    }
}
