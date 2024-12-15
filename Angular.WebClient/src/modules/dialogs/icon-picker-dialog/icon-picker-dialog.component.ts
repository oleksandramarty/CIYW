import {Component, Inject} from '@angular/core';
import {MatButtonModule} from "@angular/material/button";
import {MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle} from "@angular/material/dialog";
import {CommonModule, CurrencyPipe} from "@angular/common";
import {SharedModule} from "../../../core/shared.module";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {DictionaryService} from "../../../core/services/dictionary.service";
import {VersionedListResponseOfIconCategoryResponse} from "../../../core/api-models/common.models";

@Component({
    selector: 'app-icon-picker-dialog',
    imports: [
        CommonModule,
        CommonLoaderComponent,
        CurrencyPipe,
        MatButtonModule,
        MatDialogTitle,
        SharedModule
    ],
    templateUrl: './icon-picker-dialog.component.html',
    styleUrls: ['./icon-picker-dialog.component.scss']
})
export class IconPickerDialogComponent {
  activeIndex: number = 0;

  get iconCategories(): VersionedListResponseOfIconCategoryResponse | undefined {
    return this.dictionaryService.dictionaries?.iconCategories;
  }

  constructor(
      public dialogRef: MatDialogRef<IconPickerDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: {},
      private readonly dictionaryService: DictionaryService,
  ) {
  }

    iconSelected(iconId: number): void {
        this.dialogRef.close(iconId);
    }
}
