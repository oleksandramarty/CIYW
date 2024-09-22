import { Component, Inject } from '@angular/core';
import {environment} from "../../../../core/environments/environment";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {LocalStorageService} from "../../../../core/services/local-storage.service";

@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.scss']
})
export class FooterComponent {
  constructor(
    private readonly dictionaryService: DictionaryService,
    private readonly localStorageService: LocalStorageService) {
  }

  get apiVersion(): string {
    return this.dictionaryService.settings?.apiVersion ?? 'honk';
  }

  get clientVersion(): string {
    return this.dictionaryService.settings?.clientVersion ?? 'honk';
  }

  public clearCache(): void {
    this.localStorageService.clearLocalStorageAndRefresh();
  }
}
