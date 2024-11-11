import { Component } from '@angular/core';
import {LocalStorageService} from "../../../../core/services/local-storage.service";
import {Store} from "@ngrx/store";
import {auth_clearAll} from "../../../../core/store/actions/auth.actions";
import {LoaderService} from "../../../../core/services/loader.service";
import {Router} from "@angular/router";
import {menu_toggle} from "../../../../core/store/actions/site.actions";
import {AuthService} from "../../../../core/services/auth.service";
import {Observable} from "rxjs";
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent extends BaseAuthorizeComponent{
  constructor(
      protected override readonly authService: AuthService,
      protected override readonly store: Store,
      protected override readonly snackBar: MatSnackBar,
      private readonly router: Router,
      private readonly loaderService: LoaderService,
      private readonly localStorageService: LocalStorageService) {
    super(authService, store, snackBar);
  }

  get buildVersion(): string {
    return 'honk';
  }

  public clearCache(): void {
    this.localStorageService.clearLocalStorageAndRefresh();
  }

  public resetSite(): void {
    this.store.dispatch(auth_clearAll());
    this.localStorageService.clearLocalStorageAndRefresh(true);
  }

  public turnOffLoader(): void {
    this.loaderService.isBusy = false;
  }

  public goto(url: string | undefined): void {
    this.router.navigate([`/${url ?? ''}`]);
  }

  public toggleMenu(): void {
    this.store.dispatch(menu_toggle());
  }
}
