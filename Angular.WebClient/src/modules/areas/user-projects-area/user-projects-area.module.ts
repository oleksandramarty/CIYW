import {CommonModule} from "@angular/common";
import {NgModule} from "@angular/core";
import {ReactiveFormsModule} from "@angular/forms";
import {RouterModule, Routes} from "@angular/router";
import {AppCommonModule} from "../../common/common-app/app-common.module";
import {MatDividerModule} from "@angular/material/divider";
import {UserProjectsAreaComponent} from "./user-projects-area/user-projects-area.component";
import {UserProjectsComponent} from "./user-projects/user-projects.component";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";

const routes: Routes = [
    {
        path: '',
        component: UserProjectsAreaComponent,
        children: [
            {path: 'projects', component: UserProjectsComponent}
        ]
    }
];

@NgModule({
    declarations: [
        UserProjectsAreaComponent,
        UserProjectsComponent
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,

        AppCommonModule,
        CommonLoaderComponent,

        MatDividerModule
    ],
    exports: [
        RouterModule
    ]
})
export class UserProjectsAreaModule {
}
