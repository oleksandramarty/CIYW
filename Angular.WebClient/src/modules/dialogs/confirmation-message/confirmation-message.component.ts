import { Component, Inject } from '@angular/core';
import { AppCommonInputModule } from "../../common/common-input/app-common-input.module";
import { CommonLoaderComponent } from "../../common/common-loader/common-loader.component";
import {CommonModule, CurrencyPipe, NgIf} from "@angular/common";
import { MatButtonModule } from "@angular/material/button";
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle } from "@angular/material/dialog";
import { SharedModule } from "../../../core/shared.module";

@Component({
  selector: 'app-confirmation-message',
  standalone: true,
  imports: [
    AppCommonInputModule,
    CommonModule,
    CommonLoaderComponent,
    CurrencyPipe,
    MatButtonModule,
    MatDialogTitle,
    SharedModule
  ],
  templateUrl: './confirmation-message.component.html',
  styleUrls: ['./confirmation-message.component.scss'] // Corrected property name
})
export class ConfirmationMessageComponent {
  yesBtn: string | undefined;
  noBtn: string | undefined;
  title: string | undefined;
  descriptions: string[] | undefined;
  htmlBlock: string | undefined;

  constructor(
    public dialogRef: MatDialogRef<ConfirmationMessageComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      yesBtn: string | undefined,
      noBtn: string | undefined,
      title: string | undefined,
      descriptions: string[] | undefined,
      htmlBlock: string | undefined;
    } | undefined,
  ) {
    this.yesBtn = data?.yesBtn;
    this.noBtn = data?.noBtn;
    this.title = data?.title;
    this.descriptions = data?.descriptions;
    this.htmlBlock = data?.htmlBlock;
  }
}