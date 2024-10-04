import {Component, Inject} from '@angular/core';
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {AppCommonModule} from "../../common/common-app/app-common.module";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {CurrencyPipe, NgIf} from "@angular/common";
import {MatButton} from "@angular/material/button";
import {MAT_DIALOG_DATA, MatDialogRef, MatDialogTitle} from "@angular/material/dialog";

@Component({
  selector: 'app-confirmation-message',
  standalone: true,
    imports: [
        AppCommonInputModule,
        AppCommonModule,
        CommonLoaderComponent,
        CurrencyPipe,
        MatButton,
        MatDialogTitle,
        NgIf
    ],
  templateUrl: './confirmation-message.component.html',
  styleUrl: './confirmation-message.component.scss'
})
export class ConfirmationMessageComponent {
    yesBtn: string | undefined;
    noBtn: string | undefined;
    title: string | undefined;
    description: string | undefined;

    constructor(
        public dialogRef: MatDialogRef<ConfirmationMessageComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            yesBtn: string | undefined,
            noBtn: string | undefined,
            title: string | undefined,
            description: string | undefined,
        } | undefined,
    ) {
        this.yesBtn = data?.yesBtn;
        this.noBtn = data?.noBtn;
        this.title = data?.title;
        this.description = data?.description;
    }
}