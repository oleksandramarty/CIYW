import { CommonModule, registerLocaleData } from "@angular/common";
import localeEN from '@angular/common/locales/en';
import { LOCALE_ID, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FormsModule } from "@angular/forms";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { BaseUrlInterceptor } from "../../../core/api.interceptor";
import { MAT_DATE_FORMATS } from "@angular/material/core";
import { environment } from "../../../core/environments/environment";
import { APOLLO_OPTIONS, ApolloModule } from "apollo-angular";
import { HttpLink } from 'apollo-angular/http';
import { InMemoryCache } from '@apollo/client/core';
import { AppComponent } from "./app/app.component";
import { AppCommonModule } from "../../common/common-app/app-common.module";
import { GoogleLoginProvider, SocialAuthServiceConfig } from "@abacritt/angularx-social-login";
import { LocalStorageService } from "../../../core/services/local-storage.service";
import { LocalizationService } from "../../../core/services/localization.service";
import { BaseHttpService } from "../../../core/services/base-http.service";
import { StoreModule } from "@ngrx/store";
import { reducers } from "../../../core/store/reducers";
import { EffectsModule } from "@ngrx/effects";
import { StoreDevtoolsModule } from "@ngrx/store-devtools";
import { AuthService } from "../../../core/services/auth.service";
import {PreloadAllModules, RouterModule, RouterOutlet, Routes} from "@angular/router";
import {AuthGuard} from "../../../core/auth-guard";
import {NotFoundComponent} from "../../common/common-app/not-found/not-found.component";
import {InDevelopmentComponent} from "../../common/common-in-development/in-development/in-development.component";
import {API_BASE_URL_AuthGateway, AuthClient} from "../../../core/api-clients/auth-client";
import {API_BASE_URL_Localizations, LocalizationClient} from "../../../core/api-clients/localizations-client";
import {API_BASE_URL_Expenses, ExpenseClient} from "../../../core/api-clients/expenses-client";
import {API_BASE_URL_Dictionaries, DictionaryClient} from "../../../core/api-clients/dictionaries-client";
import {BaseInitializationService} from "../../../core/services/base-initialization.service";
import {SiteSettingsService} from "../../../core/services/site-settings.service";
import {DictionaryService} from "../../../core/services/dictionary.service";
import {API_BASE_URL_AuditTrail} from "../../../core/api-clients/audit-trail-client";
import {NightSkyComponent} from "../../common/background/night-sky/night-sky.component";

export const MY_FORMATS = {
  parse: {
    dateInput: 'DD.MM.YYYY',
  },
  display: {
    dateInput: 'DD.MM.YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

registerLocaleData(localeEN, 'en');

const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'auth/sign-in' },
  { path: 'profile', pathMatch: 'full', redirectTo: 'in-development' },
  { path: 'settings', pathMatch: 'full', redirectTo: 'in-development' },
  { path: 'notifications', pathMatch: 'full', redirectTo: 'in-development' },
  {
    path: '',
    loadChildren: () => import('../expenses-area/expenses-area.module')
      .then(m => m.ExpensesAreaModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'auth',
    loadChildren: () => import('../auth-area/auth-area.module')
      .then(m => m.AuthAreaModule)
  },
  { path: 'in-development', component: InDevelopmentComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', pathMatch: 'full', redirectTo: 'not-found' },
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules }),
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,

    NightSkyComponent,

    HttpClientModule,
    AppCommonModule,
    InDevelopmentComponent,
    ApolloModule,
    StoreModule.forRoot(reducers),
    EffectsModule.forRoot([]),
    StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: environment.production })
  ],
  providers: [
    AuthService,
    BaseHttpService,
    BaseInitializationService,
    SiteSettingsService,
    DictionaryService,
    LocalizationService,
    LocalStorageService,
    AuthClient,
    LocalizationClient,
    ExpenseClient,
    DictionaryClient,
    // AuditTrailClient,
    {provide: API_BASE_URL_AuthGateway, useValue: environment.apiAuthGatewayUrl},
    {provide: API_BASE_URL_Localizations, useValue: environment.apiLocalizationsUrl},
    {provide: API_BASE_URL_Expenses, useValue: environment.apiExpensesUrl},
    {provide: API_BASE_URL_Dictionaries, useValue: environment.apiDictionariesUrl},
    {provide: API_BASE_URL_AuditTrail, useValue: environment.apiAuditTrailUrl},
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider(
              environment.googleClientApi
            )
          }
        ]
      } as SocialAuthServiceConfig,
    },
    {
      provide: APOLLO_OPTIONS,
      useFactory: (httpLink: HttpLink) => {
        return {
          cache: new InMemoryCache(),
          link: httpLink.create({
            uri: '' // environment.graphQLUrl
          })
        };
      },
      deps: [HttpLink]
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BaseUrlInterceptor,
      multi: true,
    },
    { provide: LOCALE_ID, useValue: 'en-US' },
    { provide: MAT_DATE_FORMATS, useValue: MY_FORMATS },
  ],
  bootstrap: [AppComponent],
  exports: [RouterModule, RouterOutlet]
})
export class AppModule {}
