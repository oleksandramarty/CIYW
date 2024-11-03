import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil, tap} from "rxjs";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {selectToken} from "../../../../core/store/selectors/auth.selectors";
import {BaseUnsubscribeComponent} from "../../../../core/base-components/base-unsubscribe.compoinent";

@Component({
  selector: 'app-auth-area',
  templateUrl: './auth-area.component.html',
  styleUrl: './auth-area.component.scss'
})
export class AuthAreaComponent extends BaseUnsubscribeComponent {
  constructor(
    private readonly store: Store,
    private readonly snackBar: MatSnackBar,
    private readonly router: Router
  ) {
      super();
  }

  override ngOnInit() {
    this.store.select(selectToken)
      .pipe(
        takeUntil(this.ngUnsubscribe),
        tap(token => {
          if (token) {
            this.router.navigate(['/dashboard']);
          }
        }),
        handleApiError(this.snackBar)
        ).subscribe();
  }
}
