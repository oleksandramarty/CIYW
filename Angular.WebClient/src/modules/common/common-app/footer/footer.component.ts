import { Component } from '@angular/core';
import {LocalStorageService} from "../../../../core/services/local-storage.service";
import {Store} from "@ngrx/store";
import {auth_clearAll} from "../../../../core/store/actions/auth.actions";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  constructor(
      private readonly store: Store,
      private readonly localStorageService: LocalStorageService) {
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
}
