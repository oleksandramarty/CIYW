import { Component } from '@angular/core';
import {Router, RouterOutlet} from "@angular/router";
import {BaseUnsubscribeComponent} from "../../../../core/base-components/base-unsubscribe.compoinent";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";
import {selectToken} from "../../../../core/store/selectors/auth.selectors";
import {takeUntil, tap} from "rxjs";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";

@Component({
  selector: 'app-admin-area',
  templateUrl: './admin-area.component.html',
  styleUrl: './admin-area.component.scss'
})
export class AdminAreaComponent extends BaseUnsubscribeComponent {
  constructor(
      private readonly store: Store,
      private readonly snackBar: MatSnackBar,
      private readonly router: Router
  ) {
    super();
  }
}
