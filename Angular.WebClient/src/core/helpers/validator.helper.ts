import {AbstractControl, FormGroup, ValidationErrors, ValidatorFn} from '@angular/forms';

export class CustomValidators {
    static MaxMultiSelectedValues(maxLength: number): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
            if (!control || !control.value) {
                return null;
            }
            if (Array.isArray(control?.value) && control.value.length > maxLength) {
                return {'maxMultiSelectedValues': {value: maxLength}};
            }
            if (typeof control.value === 'string' && control.value.split(',').length > maxLength) {
                return {'maxMultiSelectedValues': {value: maxLength}};
            }
            return null;
        };
    }

    static PasswordMatchValidator(form: FormGroup): null | { mismatch: true } {
        const newPassword = form.get('newPassword')?.value;
        const confirmNewPassword = form.get('confirmNewPassword')?.value;
        return newPassword === confirmNewPassword ? null : {mismatch: true};
    }
}