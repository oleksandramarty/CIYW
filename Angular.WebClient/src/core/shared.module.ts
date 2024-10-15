import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {LocalDatePipe} from "./pipes/local-date.pipe";
import {TranslateDirective} from "./directives/translation.directive";
import {CopyToClipboardDirective} from "./directives/copy-to-clipboard.directive";
import {SanitizeHtmlPipe} from "./pipes/sanitize-html.pipe";
import {TranslationPipe} from "./pipes/translation.pipe";

@NgModule({
  declarations: [
    LocalDatePipe,
    TranslateDirective,
    CopyToClipboardDirective,
    SanitizeHtmlPipe,
    TranslationPipe,
  ],
  imports: [
    CommonModule,
    FormsModule
  ],
  exports: [
    LocalDatePipe,
    TranslateDirective,
    CopyToClipboardDirective,
    SanitizeHtmlPipe,
    TranslationPipe
  ]
})
export class SharedModule { }