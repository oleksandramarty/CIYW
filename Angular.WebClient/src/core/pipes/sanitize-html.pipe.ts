import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Pipe({
    name: 'sanitizeHtml',
    standalone: false
})
export class SanitizeHtmlPipe implements PipeTransform {
    constructor(private sanitizer: DomSanitizer) {}

    transform(value: string | undefined): SafeHtml {
        return value ? this.sanitizer.bypassSecurityTrustHtml(value) : '';
    }
}