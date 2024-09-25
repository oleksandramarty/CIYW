import { Injectable } from '@angular/core';
import {Observable, Subject, switchMap, take, takeUntil} from 'rxjs';
import {map, tap} from 'rxjs/operators';
import { clearAll, setToken, setUser } from "../store/actions/auth.actions";
import { LocalizationService } from "./localization.service";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Store } from "@ngrx/store";
import { handleApiError } from "../helpers/rxjs.helper";
import {selectToken} from "../store/selectors/auth.selectors";
import {AuthClient, AuthSignInRequest, JwtTokenResponse} from "../api-clients/auth-client";
import {LocalizationClient} from "../api-clients/localizations-client";

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
    private readonly store: Store
  ) {
    this._token$ = this.store.select(selectToken);
  }

  public initialize(): void {
    const storedToken = this.getToken();
    if (storedToken) {
      this.store.dispatch(setToken({ token: storedToken }));
    }

    if (this.isAuthorized) {
      this.getCurrentUser();
    } else {
      this.clearAuthData();
      this.router.navigate(['/auth/sign-in']);
    }
  }

  public login(login: string, password: string, rememberMe: boolean, ngUnsubscribe: Subject<void>): void {
    this.authClient.auth_SignIn(new AuthSignInRequest({
        login,
        password,
        rememberMe
    })).pipe(
      takeUntil(ngUnsubscribe),
      switchMap((token) => {
        this.setToken(token);
        this.store.dispatch(setToken({ token }));
        return this.authClient.user_GetCurrentUser();
      }),
      tap((user) => {
        this.store.dispatch(setUser({ user }));
         this.localizationService.reinitialize();

        this.router.navigate(['/home']);
      }),
      handleApiError(this.snackBar, this.localizationService)
    ).subscribe();
  }

  public logout(): void {
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

  private getCurrentUser(): void {
    this.authClient.user_GetCurrentUser().pipe(
      take(1),
      tap((user) => {
        this.store.dispatch(setUser({ user }));
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

  private setToken(token: JwtTokenResponse): void {
    localStorage.setItem(this._tokenKey, JSON.stringify(token));
    this.store.dispatch(setToken({ token }));
  }

  private getToken(): JwtTokenResponse | undefined {
    const token = localStorage.getItem(this._tokenKey);
    return token ? JSON.parse(token) : undefined;
  }

  private clearAuthData(): void {
    localStorage.removeItem(this._tokenKey);
    this.store.dispatch(clearAll());
  }
}
