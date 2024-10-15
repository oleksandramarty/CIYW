import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { MatDividerModule } from "@angular/material/divider";
import { UserProjectsAreaComponent } from "./user-projects-area/user-projects-area.component";
import { UserProjectsComponent } from "./user-projects/user-projects.component";
import { CommonLoaderComponent } from "../../common/common-loader/common-loader.component";
import { UserProjectComponent } from "./user-project/user-project.component";
import { AppCommonInputModule } from "../../common/common-input/app-common-input.module";
import { MatButtonModule } from "@angular/material/button";
import { ConfirmationMessageComponent } from "../../dialogs/confirmation-message/confirmation-message.component";
import { SharedModule } from "../../../core/shared.module";
import {CommonTopMenuComponent} from "../../common/common-top-menu/common-top-menu/common-top-menu.component";
import {UserProjectExpensesComponent} from "./user-project/user-project-expenses/user-project-expenses.component";
import {
    UserProjectPlannedExpensesComponent
} from "./user-project/user-project-planned-expenses/user-project-planned-expenses.component";
import {UserProjectFavoritesComponent} from "./user-project/user-project-favorites/user-project-favorites.component";

const routes: Routes = [
    {
        path: '',
        component: UserProjectsAreaComponent,
        children: [
            { path: 'projects', component: UserProjectsComponent },
            { path: 'projects/:id', component: UserProjectComponent }
        ]
    }
];

@NgModule({
    declarations: [
        UserProjectsAreaComponent,
        UserProjectsComponent,
        UserProjectComponent,
        UserProjectExpensesComponent,
        UserProjectPlannedExpensesComponent,
        UserProjectFavoritesComponent
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,
        CommonLoaderComponent,
        ConfirmationMessageComponent,
        SharedModule,
        MatDividerModule,
        AppCommonInputModule,
        MatButtonModule,
        CommonTopMenuComponent
    ],
    exports: [
        RouterModule
    ]
})
export class UserProjectsAreaModule { }