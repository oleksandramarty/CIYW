import { CommonModule } from "@angular/common";
import { ReactiveFormsModule } from "@angular/forms";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from "@angular/material/button";
import { NgModule } from "@angular/core";
import { NotFoundComponent } from "./not-found/not-found.component";
import { HeaderComponent } from "./header/header.component";
import { MatMenuModule } from "@angular/material/menu";
import { MatSidenavModule } from "@angular/material/sidenav";
import { FooterComponent } from "./footer/footer.component";
import {SharedModule} from "../../../core/shared.module";

@NgModule({
  declarations: [
    NotFoundComponent,
    HeaderComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatSidenavModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatMenuModule,

    SharedModule
  ],
  exports: [
    NotFoundComponent,
    HeaderComponent,
    FooterComponent
  ]
})
export class AppCommonModule {}
