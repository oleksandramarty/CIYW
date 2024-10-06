import { Injectable } from '@angular/core';
import {finalize, Observable, of, Subject, switchMap, take, takeUntil} from 'rxjs';
import {map, tap} from 'rxjs/operators';
import { auth_clearAll, auth_setToken, auth_setUser } from "../store/actions/auth.actions";
import { LocalizationService } from "./localization.service";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Store } from "@ngrx/store";
import { handleApiError } from "../helpers/rxjs.helper";
import {selectToken} from "../store/selectors/auth.selectors";
import {ConfirmationMessageComponent} from "../../modules/dialogs/confirmation-message/confirmation-message.component";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {GraphQlAuthService} from "../graph-ql/graph-ql-auth.service";
import {JwtTokenResponse, UserResponse} from "../api-clients/common-module.client";
import {LoaderService} from "./loader.service";
import {AuthClient} from "../api-clients/auth-client";
import {NoComplaintService} from "./no-complaint.service";

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
    private readonly graphQlAuthService: GraphQlAuthService,
    private readonly loaderService: LoaderService,
    private readonly noComplaintService: NoComplaintService
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
      this.loaderService.isBusy = true;
      this.graphQlAuthService.signIn(login, password, rememberMe).pipe(
          takeUntil(ngUnsubscribe),
          switchMap((result) => {
            const token = result?.data?.auth_gateway_sign_in as JwtTokenResponse;
            this.auth_setToken(token);
            return this.graphQlAuthService.getCurrentUser();
          }),
          tap((result) => {
            const user = result?.data?.auth_gateway_current_user as UserResponse;
            this.store.dispatch(auth_setUser({ user }));

            this.router.navigate(['/dashboard']);
          }),
          handleApiError(this.snackBar, this.localizationService),
          finalize(() => {
            this.loaderService.isBusy = false;
          })
      ).subscribe();
    }

    this.noComplaintService.showNoComplaintModal(loginActon);
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

    this.noComplaintService.showNoComplaintModal(logoutAction);
  }

  private getCurrentUser(): void {
    this.graphQlAuthService.getCurrentUser().pipe(
      take(1),
      tap((result) => {
        const user = result?.data?.auth_gateway_current_user as UserResponse;
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
}


