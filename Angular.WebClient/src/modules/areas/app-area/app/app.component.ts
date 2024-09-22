import { Component } from '@angular/core';
import {AuthService} from "../../../../core/services/auth.service";
import {DictionaryService} from "../../../../core/services/dictionary.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {

  constructor(
    private readonly authService: AuthService,
    private readonly dictionaryService: DictionaryService
  ) {
    this.authService.initialize();
    this.dictionaryService.initialize();
  }
}
