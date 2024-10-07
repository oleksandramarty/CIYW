import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgModule } from "@angular/core";
import { PaginatorComponent } from "./paginator/paginator.component";
import { MatPaginatorModule } from "@angular/material/paginator";

@NgModule({
  declarations: [
    PaginatorComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatPaginatorModule,
  ],
  exports: [
    PaginatorComponent
  ]
})
export class AppCommonPaginatorModule {}