import {Component, OnInit} from '@angular/core';
import { Store } from '@ngrx/store';
import {Observable, tap} from 'rxjs';
import { LocalizationService } from '../../../../core/services/localization.service';
import {selectUser} from '../../../../core/store/selectors/auth.selectors';
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {AuthService} from "../../../../core/services/auth.service";
import {UserResponse} from "../../../../core/api-clients/auth-client";
import {LocaleResponse} from "../../../../core/api-clients/localizations-client";
import {DictionaryService} from "../../../../core/services/dictionary.service";

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
    {url: 'projects', title: 'MENU.PROJECTS'},
    {url: 'analytics', title: 'MENU.ANALYTICS'},
  ];
  public currentUser: UserResponse | undefined

  get isAuthorized$(): Observable<boolean> | undefined {
    return this.authService.isAuthorized$;
  }

  get locales(): LocaleResponse[] | undefined {
    return this.dictionaryService.dictionaries?.locales?.items ?? [];
  }

  get currentLocale(): LocaleResponse | undefined {
    return this.dictionaryService.currentLocale;
  }

  constructor(
    private readonly store: Store,
    private readonly snackBar: MatSnackBar,
    private router: Router,
    private readonly localizationService: LocalizationService,
    private readonly authService: AuthService,
    private readonly dictionaryService: DictionaryService,
  ) {
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

  public localeChanged(code: string | undefined): void {
    this.localizationService.localeChanged(code);
  }

  public logout() {
    this.authService.logout()
  }
}
