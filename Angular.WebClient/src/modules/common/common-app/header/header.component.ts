import {Component, OnInit} from '@angular/core';
import { Store } from '@ngrx/store';
import {Observable, tap} from 'rxjs';
import { map } from 'rxjs/operators';
import { DictionaryService } from '../../../../core/services/dictionary.service';
import {selectToken, selectUser} from '../../../../core/store/selectors/auth.selectors';
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {AuthService} from "../../../../core/services/auth.service";
import {JwtTokenResponse, UserResponse} from "../../../../core/api-clients/auth-client";
import {LocaleResponse} from "../../../../core/api-clients/localizations-client";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  public langFlags: Map<string, string> = new Map([
    ['en', '🇬🇧'], ['fr', '🇫🇷'], ['de', '🇩🇪'],
    ['ua', '🇺🇦'], ['ru', '🇷🇺'], ['es', '🇪🇸'],
    ['it', '🇮🇹'],
  ]);
  public menuItems: { url: string, title: string }[] = [
    {url: 'home', title: 'MENU.HOME'},
    {url: 'vacancies', title: 'MENU.VACANCIES'},
    {url: 'profile', title: 'MENU.PROFILE'},
    {url: 'settings', title: 'MENU.SETTINGS'},
  ];
  public currentUser: UserResponse | undefined

  private _token$: Observable<JwtTokenResponse | undefined>;

  get isAuthorized$(): Observable<boolean> {
    return this._token$.pipe(map(token => token !== null));
  }

  get locales(): LocaleResponse[] | undefined {
    return this.dictionaryService.dictionaries?.localeResponses;
  }

  get currentLocale(): LocaleResponse | undefined {
    return this.dictionaryService.currentLocale;
  }

  constructor(
    private readonly store: Store,
    private readonly snackBar: MatSnackBar,
    private router: Router,
    private readonly dictionaryService: DictionaryService,
    private readonly authService: AuthService
  ) {
    this._token$ = this.store.select(selectToken);
  }

  ngOnInit(): void {
    this.store.select(selectUser)
      .pipe(
        tap((user) => {
          this.currentUser = user;
        }),
          handleApiError(this.snackBar)
      ).subscribe();
  }

  public goto(url: string | undefined): void {
    this.router.navigate([`/${url ?? ''}`]);
  }

  public localeChanged(id: number | undefined): void {
    this.dictionaryService.localeChanged(id);
  }

  public logout() {
    this.authService.logout()
  }
}
