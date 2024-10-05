import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { ExpensesAreaComponent } from "./expenses-area/expenses-area.component";
import { ExpensesDashboardComponent } from "./expenses-dashboard/expenses-dashboard.component";
import { MatDividerModule } from "@angular/material/divider";
import { SharedModule } from "../../../core/shared.module";
import {
    ExpenseDashboardProjectsComponent
} from "./expenses-dashboard/expense-dashboard-projects/expense-dashboard-projects.component";

const routes: Routes = [
    {
        path: '',
        component: ExpensesAreaComponent,
        children: [
            { path: 'dashboard', component: ExpensesDashboardComponent },
        ]
    }
];

@NgModule({
    declarations: [
        ExpensesAreaComponent,
        ExpensesDashboardComponent,
        ExpenseDashboardProjectsComponent
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
export class ExpensesAreaModule { }