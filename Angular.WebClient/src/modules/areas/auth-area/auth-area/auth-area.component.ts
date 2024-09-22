import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil, tap} from "rxjs";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {selectToken} from "../../../../core/store/selectors/auth.selectors";

@Component({
  selector: 'app-auth-area',
  templateUrl: './auth-area.component.html',
  styleUrl: './auth-area.component.scss'
})
export class AuthAreaComponent implements OnInit{
  protected ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(
    private readonly store: Store,
    private readonly snackBar: MatSnackBar,
    private readonly router: Router
  ) {
  }

  ngOnInit() {
    this.store.select(selectToken)
      .pipe(
        takeUntil(this.ngUnsubscribe),
        tap(token => {
          if (token) {
            this.router.navigate(['/home']);
          }
        }),
        handleApiError(this.snackBar)
        ).subscribe();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
