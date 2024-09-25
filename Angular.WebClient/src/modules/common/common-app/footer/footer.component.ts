import { Component, Inject } from '@angular/core';
import {LocalStorageService} from "../../../../core/services/local-storage.service";
import {SiteSettingsService} from "../../../../core/services/site-settings.service";
import {Store} from "@ngrx/store";
import {clearAll} from "../../../../core/store/actions/auth.actions";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  constructor(
      private readonly store: Store,
      private readonly siteSettingsService: SiteSettingsService,
      private readonly localStorageService: LocalStorageService) {
  }

  get buildVersion(): string {
    return this.siteSettingsService.siteSettings?.buildVersion ?? 'honk';
  }

  public clearCache(): void {
    this.localStorageService.clearLocalStorageAndRefresh();
  }

  public resetSite(): void {
    this.store.dispatch(clearAll());
    this.localStorageService.clearLocalStorageAndRefresh(true);
  }
}
