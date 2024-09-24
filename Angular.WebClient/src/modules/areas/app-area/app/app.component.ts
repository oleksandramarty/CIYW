import { Component } from '@angular/core';
import {AuthService} from "../../../../core/services/auth.service";
import {BaseInitializationService} from "../../../../core/services/base-initialization.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {

  constructor(
    private readonly authService: AuthService,
    private readonly baseInitializationService: BaseInitializationService
  ) {
    this.authService.initialize().then(() => {
      this.baseInitializationService.initialize();
    });
  }
}
