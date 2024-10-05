import { Injectable } from '@angular/core';
import {Observable, of, Subject, switchMap, take, takeUntil} from 'rxjs';
import {map, tap} from 'rxjs/operators';
import { auth_clearAll, auth_setToken, auth_setUser } from "../store/actions/auth.actions";
import { LocalizationService } from "./localization.service";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Store } from "@ngrx/store";
import { handleApiError } from "../helpers/rxjs.helper";
import {selectToken} from "../store/selectors/auth.selectors";
import {AuthClient, AuthSignInRequest, JwtTokenResponse} from "../api-clients/auth-client";
import {LocalizationClient} from "../api-clients/localizations-client";
import {ConfirmationMessageComponent} from "../../modules/dialogs/confirmation-message/confirmation-message.component";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _tokenKey = 'honk-token';
  private _token$: Observable<JwtTokenResponse | undefined> | undefined;

  get isAuthorized(): boolean {
    return !!this.getToken() || !!this.getTokenFromStore();
  }

  get isAuthorized$(): Observable<boolean> | undefined {
    return this._token$?.pipe(map(token => !!token));
  }

  constructor(
    private readonly localizationService: LocalizationService,
    private readonly router: Router,
    private readonly snackBar: MatSnackBar,
    private readonly authClient: AuthClient,
    private readonly store: Store,
    private dialog: MatDialog,
  ) {
    this._token$ = this.store.select(selectToken);
  }

  public initialize(): void {
    const storedToken = this.getToken();
    if (storedToken) {
      this.store.dispatch(auth_setToken({ token: storedToken }));
    }

    if (this.isAuthorized) {
      this.getCurrentUser();
    } else {
      this.clearAuthData();
      this.router.navigate(['/auth/sign-in']);
    }
  }

  public login(login: string, password: string, rememberMe: boolean, ngUnsubscribe: Subject<void>): void {
    const loginActon = () => {
      this.authClient.auth_SignIn(new AuthSignInRequest({
        login,
        password,
        rememberMe
      })).pipe(
          takeUntil(ngUnsubscribe),
          switchMap((token) => {
            this.auth_setToken(token);
            this.store.dispatch(auth_setToken({ token }));
            return this.authClient.user_GetCurrentUser();
          }),
          tap((user) => {
            this.store.dispatch(auth_setUser({ user }));

            this.router.navigate(['/projects']);
          }),
          handleApiError(this.snackBar, this.localizationService)
      ).subscribe();
    }

    this.showNoComplaintModal(loginActon);
  }

  public logout(): void {
    const logoutAction = () => {
      this.authClient.auth_SignOut()
          .pipe(
              take(1),
              tap(() => {
                this.clearAuthData();
                this.router.navigate(['/auth/sign-in']);
              }),
              handleApiError(this.snackBar, this.localizationService)
          ).subscribe();
    }

    this.showNoComplaintModal(logoutAction);
  }

  private showNoComplaintModal(authAction: () => void): void {
    let dialogRef = this.getNoComplaintModal();

    dialogRef.afterClosed()
        .pipe(
            take(1),
            tap((result) => {
              if (result) {
                authAction();
              }
            }),
            handleApiError(this.snackBar)
        )
        .subscribe();
  }

  private getCurrentUser(): void {
    this.authClient.user_GetCurrentUser().pipe(
      take(1),
      tap((user) => {
        this.store.dispatch(auth_setUser({ user }));
      }),
      handleApiError(this.snackBar, this.localizationService)
    ).subscribe();
  }

  private getTokenFromStore(): JwtTokenResponse | undefined {
    let token: JwtTokenResponse | undefined;
    this.store.select(selectToken).subscribe(authState => {
      token = authState;
    }).unsubscribe();
    return token ?? undefined;
  }

  private auth_setToken(token: JwtTokenResponse): void {
    localStorage.setItem(this._tokenKey, JSON.stringify(token));
    this.store.dispatch(auth_setToken({ token }));
  }

  private getToken(): JwtTokenResponse | undefined {
    const token = localStorage.getItem(this._tokenKey);
    return token ? JSON.parse(token) : undefined;
  }

  private clearAuthData(): void {
    localStorage.removeItem(this._tokenKey);
    this.store.dispatch(auth_clearAll());
  }

  private getNoComplaintModal(): MatDialogRef<ConfirmationMessageComponent, any> {
    return this.dialog.open(ConfirmationMessageComponent, {
      width: '400px',
      maxWidth: '80vw',
      data: {
        yesBtn: 'COMMON.PROCEED',
        noBtn: 'COMMON.CANCEL',
        title: 'COMMON.WARNING',
        htmlBlock: `
        <h2 style="color: #dc3545; text-align: center;">${this.localizationService.getTranslation('COMMON.DO_NOT_STORE_ANY_SENSITIVE_DATA_HERE')}</h2>
        <p style="text-align: center"><u>${this.localizationService.getTranslation('AUTH.NO_COMPLAINTS')}</u></p>
        `
      }
    });
  }
}


