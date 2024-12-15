import {Component} from '@angular/core';
import {Store} from '@ngrx/store';
import {tap} from 'rxjs';
import {LocalizationService} from '../../../../core/services/localization.service';
import {selectUser} from '../../../../core/store/selectors/auth.selectors';
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {AuthService} from "../../../../core/services/auth.service";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {LocaleResponse, UserResponse, UserRoleEnum} from "../../../../core/api-models/common.models";
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {menu_toggle} from "../../../../core/store/actions/site.actions";
import {MenuModel, MenuModelItem} from "../../../../core/models/common/menu.model";

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
    standalone: false
})
export class HeaderComponent extends BaseAuthorizeComponent {
    public langFlags: Map<string, string> = new Map([
        ['en', '🇬🇧'], ['fr', '🇫🇷'], ['de', '🇩🇪'],
        ['ua', '🇺🇦'], ['ru', '🇷🇺'], ['es', '🇪🇸'],
        ['it', '🇮🇹'],
    ]);

    public currentUser: UserResponse | undefined;
    public headerMenu: MenuModel | undefined;

    public menuItems: { url: string, title: string }[] = [
        {url: 'dashboard', title: 'MENU.DASHBOARD'},
        {url: 'projects', title: 'MENU.PROJECTS'},
        {url: 'analytics', title: 'MENU.ANALYTICS'},
    ];

    public menuAdminItems: { url: string, title: string }[] = [
        {url: 'admin/home', title: 'MENU.HOME'},
        {url: 'admin/users', title: 'ADMIN.MENU.USERS'},
        {url: 'admin/audit-trail', title: 'ADMIN.MENU.AUDIT_TRAIL'},
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

        this.store.select(selectUser)
            .pipe(
                tap((user) => {
                    if (!!user) {
                        this.currentUser = user;
                        this.headerMenu = new MenuModel();
                        this.headerMenu.createHeaderMenu(this.currentUser.roles.findIndex(role =>
                            role.id === UserRoleEnum.Admin ||
                            role.id === UserRoleEnum.SuperAdmin ||
                            role.id === UserRoleEnum.TechnicalSupport) > -1);
                    }
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    public goto(url: string | undefined): void {
        this.router.navigate([`/${url ?? ''}`]);
    }

    public headerGoto(event: any, menuItem: MenuModelItem): void {
        if (menuItem.url) {
            this.router.navigate([`/${menuItem.url}`]);
        } else {
            event.preventDefault();
        }
    }

    public localeChanged(code: string | undefined): void {
        this.localizationService.localeChanged(code);
    }

    public logout() {
        this.authService.logout(() => {
        })
    }

    public toggleMenu(): void {
        this.store.dispatch(menu_toggle());
    }
}
