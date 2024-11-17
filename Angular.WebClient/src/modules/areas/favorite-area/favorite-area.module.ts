import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
import { MatDividerModule } from "@angular/material/divider";
import { SharedModule } from "../../../core/shared.module";
import {CommonFavoriteComponent} from "../../common/common-favorite/common-favorite.component";
import {FavoriteLinksComponent} from "./favorite-links/favorite-links.component";
import {FavoriteAreaComponent} from "./favorite-area/favorite-area.component";

const routes: Routes = [
    {
        path: '',
        component: FavoriteAreaComponent,
        children: [
            { path: 'favorites', component: FavoriteLinksComponent },
        ]
    }
];

@NgModule({
    declarations: [
        FavoriteAreaComponent,
        FavoriteLinksComponent
    ],
    imports: [
        RouterModule.forChild(routes),
        CommonModule,
        ReactiveFormsModule,
        SharedModule,
        MatDividerModule,
        CommonFavoriteComponent
    ],
    exports: [
        RouterModule
    ]
})
export class FavoriteAreaModule { }