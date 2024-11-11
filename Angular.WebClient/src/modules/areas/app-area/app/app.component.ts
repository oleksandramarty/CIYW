import {Component, HostListener} from '@angular/core';
import {AuthService} from "../../../../core/services/auth.service";
import {BaseInitializationService} from "../../../../core/services/base-initialization.service";
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";
import {fadeInOut, slideInFromLeft, slideInOut} from "../../../../core/animations/animations";
import {selectMenuState} from "../../../../core/store/selectors/site.selectors";
import {map, takeUntil} from "rxjs/operators";
import {menu_toggle} from "../../../../core/store/actions/site.actions";
import {of, tap} from "rxjs";
import {MenuModel, MenuModelItem} from "../../../../core/models/common/menu.model";
import {Router} from "@angular/router";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    animations: [fadeInOut, slideInOut, slideInFromLeft]
})
export class AppComponent extends BaseAuthorizeComponent {
    private _isSideMenuVisible$ = this.store.select(selectMenuState);

    public sideMenu: MenuModel | undefined;

    @HostListener('document:click', ['$event'])
    onDocumentClick(event: MouseEvent) {
        const target = event.target as HTMLElement;
        const sideMenu = document.querySelector('.side-menu.open');
        const toggleMenuButtons = document.querySelectorAll('.toggle-menu-button');
        const isToggleMenuButtonClicked = Array.from(toggleMenuButtons).some(button => button.contains(target));

        if (
            sideMenu &&
            !sideMenu.contains(target) &&
            !isToggleMenuButtonClicked
        ) {
            this.store.dispatch(menu_toggle());
        }
    }

    constructor(
        protected override readonly authService: AuthService,
        protected override readonly store: Store,
        protected override readonly snackBar: MatSnackBar,
        private readonly baseInitializationService: BaseInitializationService,
        private readonly router: Router
    ) {
        super(authService, store, snackBar);
        this.authService.initialize();
        this.baseInitializationService.initialize();
    }

    override ngOnInit() {
        super.ngOnInit();
        this._isSideMenuVisible$
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap(isSideMenuVisible => {
                    if (!this.sideMenu && isSideMenuVisible !== undefined) {
                        this.sideMenu = new MenuModel();
                        this.sideMenu.createSideMenu(!!isSideMenuVisible);
                    }
                }))
            .subscribe();
        document.addEventListener('click', this.onDocumentClick.bind(this));
    }

    override ngOnDestroy() {
        super.ngOnDestroy();
        document.removeEventListener('click', this.onDocumentClick.bind(this));
    }

    get isSideMenuVisible$() {
        return this._isSideMenuVisible$?.pipe(map(isSideMenuVisible => !!isSideMenuVisible)) ?? of(false);
    }

    public toggleMenuItem(menuItem: MenuModelItem) {
        if (!!menuItem.menuItems) {
            menuItem.isOpen = !menuItem.isOpen;
        } else {
            this.store.dispatch(menu_toggle());
            this.router.navigate([`/${menuItem.url ?? ''}`]);
        }
    }

    public logout() {
        this.store.dispatch(menu_toggle());
        this.authService.logout()
    }
}