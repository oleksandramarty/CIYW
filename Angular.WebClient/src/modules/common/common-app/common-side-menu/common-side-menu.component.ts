import {Component, HostListener} from '@angular/core';
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {selectMenuState} from "../../../../core/store/selectors/site.selectors";
import {MenuModel, MenuModelItem} from "../../../../core/models/common/menu.model";
import {menu_toggle} from "../../../../core/store/actions/site.actions";
import {AuthService} from "../../../../core/services/auth.service";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {map, takeUntil} from "rxjs/operators";
import {of, tap} from "rxjs";
import {fadeInOut, slideInFromLeft, slideInOut} from "../../../../core/animations/animations";

@Component({
    selector: 'app-common-side-menu',
    templateUrl: './common-side-menu.component.html',
    styleUrl: './common-side-menu.component.scss',
    animations: [fadeInOut, slideInOut, slideInFromLeft],
    standalone: false
})
export class CommonSideMenuComponent extends BaseAuthorizeComponent {
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
      private readonly router: Router
  ) {
    super(authService, store, snackBar);
  }

  override ngOnInit() {
    super.ngOnInit();
    this._isSideMenuVisible$
        .pipe(
            takeUntil(this.ngUnsubscribe),
            tap(isSideMenuVisible => {
              if (!this.sideMenu && isSideMenuVisible !== undefined) {
                this.sideMenu = new MenuModel();
                this.sideMenu.createSideMenu();
              }
            }))
        .subscribe();

    this.isAdminAreaAvailable$
        ?.pipe(
            takeUntil(this.ngUnsubscribe),
            tap(isAdminAreaAvailable => {
              if (!this.sideMenu && isAdminAreaAvailable !== undefined && !!isAdminAreaAvailable) {
                this.sideMenu = new MenuModel();
                this.sideMenu.createAdminSideMenu();
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
    this.authService.logout(() => {
      this.store.dispatch(menu_toggle());
    })
  }
}
