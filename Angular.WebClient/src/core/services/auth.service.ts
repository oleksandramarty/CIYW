import { Injectable } from '@angular/core';
import { Subject, switchMap, take, takeUntil } from 'rxjs';
import { tap } from 'rxjs/operators';
import { API_ROUTES } from '../helpers/api-route.helper';
import { clearAll, setToken, setUser } from "../store/actions/auth.actions";
import { LocalizationData } from "../models/common/dictionary.model";
import { BaseHttpService } from "./base-http.service";
import { DictionaryService } from "./dictionary.service";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { Store } from "@ngrx/store";
import { handleApiError } from "../helpers/rxjs.helper";
import {UserResponse} from "../models/users/user.model";
import {selectToken} from "../store/selectors/auth.selectors";
import {JwtTokenResponse} from "../models/auth/account.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _tokenKey = 'honk-token';

  get isAuthorized(): boolean {
    return !!this.getToken() || !!this.getTokenFromStore();
  }

  constructor(
    private readonly baseHttpService: BaseHttpService,
    private readonly dictionaryService: DictionaryService,
    private readonly router: Router,
    private readonly snackBar: MatSnackBar,
    private readonly store: Store
  ) {}

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
    this.baseHttpService.post<JwtTokenResponse>(
      API_ROUTES.USER_API.ACCOUNT.LOGIN,
      { login, password, rememberMe }
    ).pipe(
      takeUntil(ngUnsubscribe),
      switchMap((token) => {
        this.setToken(token);
        this.store.dispatch(setToken({ token }));
        return this.baseHttpService.get<UserResponse>(API_ROUTES.USER_API.USER.CURRENT);
      }),
      switchMap((user) => {
        this.store.dispatch(setUser({ user }));
        return this.baseHttpService.get<LocalizationData>(API_ROUTES.DATA_API.LOCALIZATIONS.GET);
      }),
      tap((data) => {
        this.dictionaryService.updateLocalizations(data);
      }),
      tap(() => {
        this.router.navigate(['/home']);
      }),
      handleApiError(this.snackBar, this.dictionaryService)
    ).subscribe();
  }

  public logout(): void {
    this.baseHttpService.get<boolean>(API_ROUTES.USER_API.ACCOUNT.LOGOUT)
      .pipe(
        take(1),
        tap(() => {
          this.clearAuthData();
          this.router.navigate(['/auth/sign-in']);
        }),
        handleApiError(this.snackBar, this.dictionaryService)
      ).subscribe();
  }

  private getCurrentUser(): void {
    this.baseHttpService.get<UserResponse>(API_ROUTES.USER_API.USER.CURRENT).pipe(
      take(1),
      tap((user) => {
        this.store.dispatch(setUser({ user }));
      }),
      handleApiError(this.snackBar, this.dictionaryService)
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
