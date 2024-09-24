import { Component, Inject } from '@angular/core';
import {LocalStorageService} from "../../../../core/services/local-storage.service";
import {SiteSettingsService} from "../../../../core/services/site-settings.service";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  constructor(
    private readonly siteSettingsService: SiteSettingsService,
    private readonly localStorageService: LocalStorageService) {
  }

  get buildVersion(): string {
    return this.siteSettingsService.siteSettings?.buildVersion ?? 'honk';
  }

  public clearCache(): void {
    this.localStorageService.clearLocalStorageAndRefresh();
  }
}
