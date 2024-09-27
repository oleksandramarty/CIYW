import { Component } from '@angular/core';
import {LocalStorageService} from "../../../../core/services/local-storage.service";
import {Store} from "@ngrx/store";
import {clearAll} from "../../../../core/store/actions/auth.actions";
import {environment} from "../../../../core/environments/environment";

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
    return environment.buildVersion ?? 'honk';
  }

  public clearCache(): void {
    this.localStorageService.clearLocalStorageAndRefresh();
  }

  public resetSite(): void {
    this.store.dispatch(clearAll());
    this.localStorageService.clearLocalStorageAndRefresh(true);
  }
}
