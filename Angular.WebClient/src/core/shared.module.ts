
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {LocalDatePipe} from "./pipes/local-date.pipe";
import {TranslateDirective} from "./directives/translation.directive";
import {CopyToClipboardDirective} from "./directives/copy-to-clipboard.directive";

@NgModule({
  declarations: [
    LocalDatePipe,
    TranslateDirective,
    CopyToClipboardDirective
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    CommonModule,
    FormsModule,
    LocalDatePipe,
    TranslateDirective,
    CopyToClipboardDirective
  ]
})
export class SharedModule { }