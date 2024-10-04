import {CommonModule} from "@angular/common";
import {NgModule} from "@angular/core";
import {MatFormFieldModule} from "@angular/material/form-field";
import {ReactiveFormsModule} from "@angular/forms";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatIconModule} from "@angular/material/icon";
import {MatDialogModule} from "@angular/material/dialog";
import {MatInputModule} from "@angular/material/input";
import {MatCardModule} from "@angular/material/card";
import {MatButtonModule} from "@angular/material/button";
import {MatSortModule} from "@angular/material/sort";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatSelectModule} from "@angular/material/select";
import {AuthAreaComponent} from "./auth-area/auth-area.component";
import {MatGridListModule} from "@angular/material/grid-list";
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {MatDividerModule} from "@angular/material/divider";
import {AuthForgotComponent} from "./auth-forgot/auth-forgot.component";
import {AppCommonModule} from "../../common/common-app/app-common.module";
import {RouterModule, Routes} from "@angular/router";
import {AuthSignInComponent} from "./auth-sign-in/auth-sign-in.component";
import {AuthSignUpComponent} from "./auth-sign-up/auth-sign-up.component";
import {AuthRestoreComponent} from "./auth-restore/auth-restore.component";
import {SharedModule} from "../../../core/shared.module";

const routes: Routes = [
    {
        path: '',
        component: AuthAreaComponent,
        children: [
            {path: '', pathMatch: 'full', redirectTo: 'sign-in'},
            {path: 'sign-in', component: AuthSignInComponent},
            {path: 'sign-up', component: AuthSignUpComponent},
            {path: 'forgot', component: AuthForgotComponent},
            {path: 'restore', component: AuthRestoreComponent}
        ]
    }
];

@NgModule({
    declarations: [
        AuthAreaComponent,
        AuthSignInComponent,
        AuthSignUpComponent,
        AuthForgotComponent,
        AuthRestoreComponent
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,
        AppCommonModule,
        MatInputModule,
        MatDatepickerModule,
        MatIconModule,
        MatDialogModule,
        MatFormFieldModule,
        MatCardModule,
        MatButtonModule,
        MatToolbarModule,
        MatSortModule,
        MatPaginatorModule,
        MatSelectModule,
        MatGridListModule,
        AppCommonInputModule,
        MatDividerModule,

        SharedModule,
    ],
    exports: [
        AuthAreaComponent,
        AuthSignInComponent,
        AuthSignUpComponent,
        AuthForgotComponent,
        AuthRestoreComponent,
        RouterModule
    ]
})
export class AuthAreaModule {
}
