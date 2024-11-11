import { Component } from '@angular/core';
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {AuthService} from "../../../../core/services/auth.service";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.scss'
})
export class NotFoundComponent extends BaseAuthorizeComponent {
  constructor(
      protected override readonly authService: AuthService,
      protected override readonly store: Store,
      protected override readonly snackBar: MatSnackBar,
  ) {
    super(authService, store, snackBar);
  }
}
