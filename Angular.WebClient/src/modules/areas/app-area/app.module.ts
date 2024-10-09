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
import {APOLLO_NAMED_OPTIONS, APOLLO_OPTIONS, ApolloModule} from "apollo-angular";
import { HttpLink } from 'apollo-angular/http';
import {InMemoryCache} from '@apollo/client/core';
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
import { PreloadAllModules, RouterModule, RouterOutlet, Routes } from "@angular/router";
import { AuthGuard } from "../../../core/auth-guard";
import { NotFoundComponent } from "../../common/common-app/not-found/not-found.component";
import { InDevelopmentComponent } from "../../common/common-in-development/in-development/in-development.component";
import { BaseInitializationService } from "../../../core/services/base-initialization.service";
import { SiteSettingsService } from "../../../core/services/site-settings.service";
import { DictionaryService } from "../../../core/services/dictionary.service";
import { LocalDatePipe } from "../../../core/pipes/local-date.pipe";
import { SharedModule } from "../../../core/shared.module";
import {NightSkyComponent} from "../../common/background/night-sky/night-sky.component";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {LoaderService} from "../../../core/services/loader.service";
import {UserProjectsService} from "../../../core/services/entity-services/user-projects.service";
import {GraphQlDictionariesService} from "../../../core/graph-ql/services/graph-ql-dictionaries.service";
import {GraphQlService} from "../../../core/graph-ql/graph-ql.service";
import {GraphQlAuthService} from "../../../core/graph-ql/services/graph-ql-auth.service";
import {CommonDialogService} from "../../../core/services/common-dialog.service";
import {GraphQlExpensesService} from "../../../core/graph-ql/services/graph-ql-expenses.service";
import {GraphQlLocalizationsService} from "../../../core/graph-ql/services/graph-ql-localizations.service";

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

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'auth/sign-in' },
  { path: 'profile', pathMatch: 'full', redirectTo: 'in-development' },
  { path: 'settings', pathMatch: 'full', redirectTo: 'in-development' },
  { path: 'notifications', pathMatch: 'full', redirectTo: 'in-development' },
  {
    path: '',
    loadChildren: () => import('../user-projects-area/user-projects-area.module')
      .then(m => m.UserProjectsAreaModule),
    canActivate: [AuthGuard]
  },
  {
    path: '',
    loadChildren: () => import('../dashboard-area/dashboard-area.module')
      .then(m => m.DashboardAreaModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'auth',
    loadChildren: () => import('../auth-area/auth-area.module')
      .then(m => m.AuthAreaModule)
  },
  {
    path: 'users',
    loadChildren: () => import('../user-area/user-area.module')
      .then(m => m.UserAreaModule)
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
    RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules}),
    BrowserModule,
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,
    SharedModule,
    HttpClientModule,
    AppCommonModule,
    NightSkyComponent,
    InDevelopmentComponent,
    ApolloModule,
    StoreModule.forRoot(reducers),
    EffectsModule.forRoot([]),
    StoreDevtoolsModule.instrument({maxAge: 25, logOnly: environment.production}),
    CommonLoaderComponent
  ],
  providers: [
    CommonDialogService,
    AuthService,
    BaseHttpService,
    BaseInitializationService,
    SiteSettingsService,
    DictionaryService,
    LoaderService,
    LocalizationService,
    LocalStorageService,
    UserProjectsService,
    {
      provide: APOLLO_NAMED_OPTIONS,
      useFactory: (httpLink: HttpLink) => {
        return {
          authGateway: {cache: new InMemoryCache(), link: httpLink.create({ uri: `${environment.apiAuthGatewayUrl}/graphql` }),},
          localizations: {cache: new InMemoryCache(), link: httpLink.create({ uri: `${environment.apiLocalizationsUrl}/graphql` }),},
          expenses: {cache: new InMemoryCache(), link: httpLink.create({ uri: `${environment.apiExpensesUrl}/graphql` }),},
          dictionaries: {cache: new InMemoryCache(), link: httpLink.create({ uri: `${environment.apiDictionariesUrl}/graphql` }),},
          auditTrail: {cache: new InMemoryCache(), link: httpLink.create({ uri: `${environment.apiAuditTrailUrl}/graphql` }),},
        };
      },
      deps: [HttpLink],
    },
    GraphQlService,
    GraphQlDictionariesService,
    GraphQlAuthService,
    GraphQlExpensesService,
    GraphQlLocalizationsService,
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
  exports: [RouterModule, RouterOutlet, LocalDatePipe]
})
export class AppModule {}