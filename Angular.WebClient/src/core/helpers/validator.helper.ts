import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export class CustomValidators {
    static MaxMultiSelectedValues(maxLength: number): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            if (control?.value?.length > maxLength) {
                return { 'maxMultiSelectedValues': { value: maxLength } };
            }
            return null;
        };
    }
}