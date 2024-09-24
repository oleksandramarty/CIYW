import { Component, Inject } from '@angular/core';
import {LocalizationService} from "../../../../core/services/localization.service";
import {LocalStorageService} from "../../../../core/services/local-storage.service";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  constructor(
    private readonly localizationService: LocalizationService,
    private readonly localStorageService: LocalStorageService) {
  }

  get apiVersion(): string {
    return this.localizationService.settings?.apiVersion ?? 'honk';
  }

  get clientVersion(): string {
    return this.localizationService.settings?.clientVersion ?? 'honk';
  }

  public clearCache(): void {
    this.localStorageService.clearLocalStorageAndRefresh();
  }
}
