import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { MatDividerModule } from "@angular/material/divider";
import { SharedModule } from "../../../core/shared.module";
import {DashboardAreaComponent} from "./dashboard-area/dashboard-area.component";
import {DashboardComponent} from "./dashboard/dashboard.component";
import {DashboardUserProjectsComponent} from "./dashboard/dashboard-user-projects/dashboard-user-projects.component";

const routes: Routes = [
    {
        path: '',
        component: DashboardAreaComponent,
        children: [
            { path: 'dashboard', component: DashboardComponent },
        ]
    }
];

@NgModule({
    declarations: [
        DashboardAreaComponent,
        DashboardComponent,
        DashboardUserProjectsComponent
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,
        SharedModule,
        MatDividerModule
    ],
    exports: [
        RouterModule
    ]
})
export class DashboardAreaModule { }