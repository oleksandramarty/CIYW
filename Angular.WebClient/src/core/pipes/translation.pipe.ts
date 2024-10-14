import { Pipe, PipeTransform } from '@angular/core';
import { LocalizationService } from '../services/localization.service';

@Pipe({
    name: 'translation',
    pure: false
})
export class TranslationPipe implements PipeTransform {
    constructor(private localizationService: LocalizationService) {}

    transform(key: string | undefined): string {
        if (!key) {
            return '';
        }

        const translation = this.localizationService.getTranslation(key);
        return translation || key;
    }
}