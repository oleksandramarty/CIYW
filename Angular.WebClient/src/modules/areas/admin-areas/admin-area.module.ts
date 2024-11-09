import {RouterModule, Routes} from "@angular/router";
import {AuthAreaComponent} from "../auth-area/auth-area/auth-area.component";
import {AuthSignInComponent} from "../auth-area/auth-sign-in/auth-sign-in.component";
import {AuthSignUpComponent} from "../auth-area/auth-sign-up/auth-sign-up.component";
import {AuthForgotComponent} from "../auth-area/auth-forgot/auth-forgot.component";
import {AuthRestoreComponent} from "../auth-area/auth-restore/auth-restore.component";
import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatIconModule} from "@angular/material/icon";
import {MatDialogModule} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatCardModule} from "@angular/material/card";
import {MatButtonModule} from "@angular/material/button";
import {MatToolbarModule} from "@angular/material/toolbar";
import {MatSortModule} from "@angular/material/sort";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatSelectModule} from "@angular/material/select";
import {MatGridListModule} from "@angular/material/grid-list";
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {MatDividerModule} from "@angular/material/divider";
import {SharedModule} from "../../../core/shared.module";
import {AdminAreaComponent} from "./admin-area/admin-area.component";
import {AuthGuard} from "../../../core/auth-guard";

const routes: Routes = [
    {
        path: '',
        component: AdminAreaComponent
    },
    {
        path: 'audit-trail',
        loadChildren: () => import('./areas/admin-audit-trail-area/admin-audit-trail-area.module')
            .then(m => m.AdminAuditTrailAreaModule),
        canActivate: [AuthGuard]
    },
];

@NgModule({
    declarations: [
        AdminAreaComponent,
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,
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
        RouterModule
    ]
})
export class AdminAreaModule {
}