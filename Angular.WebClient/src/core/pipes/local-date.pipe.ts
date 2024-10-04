import {Pipe, PipeTransform} from '@angular/core';
import {formatDate} from "@angular/common";

@Pipe({
    name: 'localDate'
})
export class LocalDatePipe implements PipeTransform {
    // private getUserTimeShift(): number {
    //     const date = new Date();
    //     return -date.getTimezoneOffset();
    // }
    //
    // private applyTimeShift(date: Date): Date {
    //     date.setMinutes(date.getMinutes() - this.getUserTimeShift());
    //     return date;
    // }

    transform(value: string | Date, format: string = 'MMMM dd, yyyy', local: boolean = true): string {
        if (!value) return '';
        let date = new Date(value);
        return formatDate(date, format, 'en-US', Intl.DateTimeFormat().resolvedOptions().timeZone);
    }
}