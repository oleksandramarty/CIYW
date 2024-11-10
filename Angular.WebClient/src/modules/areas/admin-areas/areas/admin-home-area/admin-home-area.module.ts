import {RouterModule, Routes} from "@angular/router";
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
import {AppCommonInputModule} from "../../../../common/common-input/app-common-input.module";
import {MatDividerModule} from "@angular/material/divider";
import {SharedModule} from "../../../../../core/shared.module";
import {AdminHomeAreaComponent} from "./admin-home-area/admin-home-area.component";
import {AdminHomeComponent} from "./admin-home/admin-home.component";

const routes: Routes = [
    {
        path: '',
        component: AdminHomeAreaComponent,
        children: [
            {path: 'admin', pathMatch: 'full', redirectTo: 'home'},
            {path: '', component: AdminHomeComponent},
        ]
    }
];

@NgModule({
    declarations: [
        AdminHomeAreaComponent,
        AdminHomeComponent
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
export class AdminHomeAreaModule {
}