import {ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, signal} from '@angular/core';
import {AbstractControl, FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import {DataItem} from "../../../../core/models/common/data-item.model";
import {Subject, takeUntil, tap} from "rxjs";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";

type InputType =
  'input' |
  'select' |
  'multiselect' |
  'textarea' |
  'password' |
  'datepicker' |
  'rangedatapicker' |
  'radio' |
  'checkbox' |
  'toggle' |
  'slider' |
  null;

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  styleUrls: ['./input.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InputComponent implements OnInit, OnDestroy{
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  @Input() className: string | undefined;
  @Input() type: InputType = 'input';
  @Input() appearance: 'fill' | 'outline' = 'outline';
  @Input() placeholder: string | undefined;
  @Input() hint: string | undefined;
  @Input() icon: string | undefined;
  @Input() label: string | undefined;
  @Input() leftLabel: string | undefined;
  @Input() rightLabel: string | undefined;
  @Input() rows: number | undefined;
  @Input() cols: number | undefined;
  @Input() minDate: Date | undefined;
  @Input() maxDate: Date | undefined;
  @Input() formGroup: FormGroup | undefined;
  @Input() controlName: string = 'inputControl';
  @Input() dataItems: DataItem[] | undefined;
  @Input() mode: 'inline' | 'block' | null = 'block';
  @Input() sliderStep: number | undefined;
  @Input() sliderStepSuffix: string | undefined;
  @Input() leftValue: any | undefined;
  @Input() rightValue: any | undefined;
  @Input() align: 'left' | 'center' | 'right' = 'left';

  internalFormGroup: FormGroup | undefined;

  constructor(private snackBar: MatSnackBar) {
  }

  public ngOnInit(): void {
    if (!this.formGroup) {
      this.formGroup = new FormGroup({
        inputControl: new FormControl(null)
      });
    }

    if (['rangedatapicker','slider'].findIndex(x => x === this.type) > -1) {
      this.internalFormGroup = new FormGroup({
        left: new FormControl(this.leftValue),
        right: new FormControl(this.rightValue)
      });

      this.internalFormGroup.valueChanges
        .pipe(
          takeUntil(this.ngUnsubscribe),
          tap(() => {
            this.currentControl.setValue(`${this.internalFormGroup!.get('left')?.value},${this.internalFormGroup!.get('right')?.value}`)
          }),
          handleApiError(this.snackBar),
        ).subscribe();
    }
  }

  public ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  get currentControl(): any {
    return this.formGroup?.get(this.controlName);
  }

  get currenValue(): any {
    return this.formGroup?.get(this.controlName)?.value ?? undefined;
  }

  get isRequired(): boolean {
    return this.currentControl?.hasValidator(Validators.required) ?? false;
  }

  get maxLength(): number | null {
    const maxLengthValidator = this.currentControl?.validator ? this.currentControl.validator({} as AbstractControl) : null;
    return maxLengthValidator && maxLengthValidator.maxLength ? maxLengthValidator.maxLength : null;
  }

  get minLength(): number | null {
    const minLengthValidator = this.currentControl?.validator ? this.currentControl.validator({} as AbstractControl) : null;
    return minLengthValidator && minLengthValidator.minLength ? minLengthValidator.maxLength : null;
  }

  get isDisabled(): boolean {
    return this.currentControl.disabled ?? false;
  }

  public formatLabel(value: number): string {
    if (value >= (this.sliderStep ?? 1)) {
      return Math.round(value / (this.sliderStep ?? 1)) + (this.sliderStepSuffix ?? '');
    }
    return `${value}`;
  }

  protected readonly value = signal('');
  hide = signal(true);

  protected onInput(event: Event) {
    this.value.set((event.target as HTMLInputElement).value);
  }

  clickEvent(event: MouseEvent) {
    this.hide.set(!this.hide());
    event.stopPropagation();
  }
}
