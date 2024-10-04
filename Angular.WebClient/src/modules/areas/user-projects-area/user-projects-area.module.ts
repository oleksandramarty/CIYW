import {CommonModule} from "@angular/common";
import {NgModule} from "@angular/core";
import {ReactiveFormsModule} from "@angular/forms";
import {RouterModule, Routes} from "@angular/router";
import {AppCommonModule} from "../../common/common-app/app-common.module";
import {MatDividerModule} from "@angular/material/divider";
import {UserProjectsAreaComponent} from "./user-projects-area/user-projects-area.component";
import {UserProjectsComponent} from "./user-projects/user-projects.component";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {UserProjectComponent} from "./user-project/user-project.component";
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {MatButton} from "@angular/material/button";
import {ConfirmationMessageComponent} from "../../dialogs/confirmation-message/confirmation-message.component";
import {SharedModule} from "../../../core/shared.module";

const routes: Routes = [
    {
        path: '',
        component: UserProjectsAreaComponent,
        children: [
            {path: 'projects', component: UserProjectsComponent},
            {path: 'projects/:id', component: UserProjectComponent}
        ]
    }
];

@NgModule({
    declarations: [
        UserProjectsAreaComponent,
        UserProjectsComponent,
        UserProjectComponent
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,

        AppCommonModule,
        CommonLoaderComponent,
        ConfirmationMessageComponent,

        SharedModule,

        MatDividerModule,
        AppCommonInputModule,
        MatButton
    ],
    exports: [
        RouterModule
    ]
})
export class UserProjectsAreaModule {
}
