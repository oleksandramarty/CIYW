import {Component, Inject} from '@angular/core';
import {MatButton, MatButtonModule} from "@angular/material/button";
import {MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle} from "@angular/material/dialog";
import {CommonModule, CurrencyPipe} from "@angular/common";
import {SharedModule} from "../../../core/shared.module";
import {IconDataItem, IconDataItemPicker} from "../../../core/models/common/icon-data-item.model";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";

@Component({
  selector: 'app-icon-picker',
  standalone: true,
  imports: [
    CommonModule,
    CommonLoaderComponent,
    CurrencyPipe,
    MatButtonModule,
    MatDialogTitle,
    SharedModule
  ],
  templateUrl: './icon-picker.component.html',
  styleUrl: './icon-picker.component.scss'
})
export class IconPickerComponent {
  iconDataItems: IconDataItemPicker = new IconDataItemPicker();
  activeIndex: number = 0;

  constructor(
      public dialogRef: MatDialogRef<IconPickerComponent>,
      @Inject(MAT_DIALOG_DATA) public data: {},
  ) {
  }

    iconSelected(icon: string): void {
        this.dialogRef.close(icon);
    }
}
