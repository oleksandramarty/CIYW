import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { MatDividerModule } from "@angular/material/divider";
import { SharedModule } from "../../../core/shared.module";
import {UserAreaComponent} from "./user-area/user-area.component";
import {UserSettingsComponent} from "./user-settings/user-settings.component";
import {UserNotificationsComponent} from "./user-notifications/user-notifications.component";
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {MatButton} from "@angular/material/button";

const routes: Routes = [
    {
        path: '',
        component: UserAreaComponent,
        children: [
            { path: 'settings', component: UserSettingsComponent },
            { path: 'notifications', component: UserNotificationsComponent },
        ]
    }
];

@NgModule({
    declarations: [
        UserAreaComponent,
        UserSettingsComponent,
        UserNotificationsComponent
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,
        SharedModule,
        MatDividerModule,
        AppCommonInputModule,
        MatButton
    ],
    exports: [
        RouterModule
    ]
})
export class UserAreaModule { }