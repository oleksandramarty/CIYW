import {ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, signal} from '@angular/core';
import {AbstractControl, FormArray, FormControl, FormGroup, Validators} from "@angular/forms";
import {DataItem} from "../../../../core/models/common/data-item.model";
import {debounceTime, Observable, startWith, Subject, takeUntil, tap} from "rxjs";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {map} from "rxjs/operators";
import {LocalizationService} from "../../../../core/services/localization.service";

type InputType =
    'input' |
    'select' |
    'multiselect' |
    'autocomplete' |
    'multiautocomplete' |
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
export class InputComponent implements OnInit, OnDestroy {
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
    @Input() maxLength: number | undefined;
    @Input() minLength: number | undefined;
    @Input() multiMaxCount: number | undefined;
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

    filteredDataItems: Observable<DataItem[] | undefined> | undefined;
    selectedDataItems: DataItem[] = [];

    internalFormGroup: FormGroup | undefined;

    constructor(
        private readonly localizationService: LocalizationService,
        private snackBar: MatSnackBar
    ) {
        if (this.dataItems) {
            this.dataItems.sort((a, b) => (b.isImportant ? 1 : 0) - (a.isImportant ? 1 : 0));
        }

        this.displayFn = this.displayFn.bind(this);
    }

    public ngOnInit(): void {
        if (!this.formGroup) {
            this.formGroup = new FormGroup({
                inputControl: new FormControl(null)
            });
        }

        if (this.type === 'autocomplete' || this.type === 'multiautocomplete') {
            this.internalFormGroup = new FormGroup({
                autocomplete: new FormControl(this.dataItems?.find(x => x.id === this.currentValue))
            });

            this.filteredDataItems = this.internalFormGroup?.get('autocomplete')?.valueChanges.pipe(
                startWith(''),
                debounceTime(300),
                map(value => {
                    const name = typeof value === 'string' ? value : value?.name;
                    return name ? this._filterAutoComplete(name as string) : this.dataItems?.slice();
                }),
            );
        }

        if (['rangedatapicker', 'slider'].findIndex(x => x === this.type) > -1) {
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

    get currentValue(): any {
        return this.formGroup?.get(this.controlName)?.value ?? undefined;
    }

    get isRequired(): boolean {
        return this.currentControl?.hasValidator(Validators.required) ?? false;
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

    displayFn(dataItem: DataItem): string {
        return this.localizationService.getTranslation(dataItem?.name) ?? '';
    }

    private _filterAutoComplete(name: string): DataItem[] | undefined {
        const filterValue = name.toLowerCase();

        if (this.formGroup?.get(this.controlName)!.value ===
            this.internalFormGroup?.get('autocomplete')?.value?.id) {
            return this.dataItems;
        }

        return this.dataItems?.filter(item =>
            item.filteredFields?.some(field => field?.toLowerCase().includes(filterValue)) ||
            item.name?.toLowerCase().includes(filterValue));
    }

    public onAutoCompleteChecked(): void {
        if (this.type === 'multiautocomplete') {
            if (this.internalFormGroup?.get('autocomplete')?.value) {
                const newItem = this.internalFormGroup?.get('autocomplete')?.value;
                if (!this.selectedDataItems.some(item => item.id === newItem.id)) {
                    this.selectedDataItems.push(newItem);
                }
                this.formGroup?.get(this.controlName)?.setValue(this.selectedDataItems.map(x => x.id).join(','));
            }
            this.internalFormGroup?.get('autocomplete')?.setValue(null);

            this.internalFormGroup?.get('autocomplete')?.setErrors(this.formGroup?.get(this.controlName)?.errors ?? null);
        }

        if (this.type === 'autocomplete') {
            this.formGroup?.get(this.controlName)?.setValue(this.internalFormGroup?.get('autocomplete')?.value?.id);
        }
    }

    removeOnAutoComplete(item: DataItem): void {
        const index = this.selectedDataItems?.indexOf(item);
        if (index >= 0) {
            this.selectedDataItems.splice(index, 1);
            this.formGroup?.get(this.controlName)?.setValue(this.selectedDataItems.map(x => x.id).join(','));
            this.internalFormGroup?.get('autocomplete')?.setErrors(this.formGroup?.get(this.controlName)?.errors ?? null);
        }
    }

    toggleChildrenSelection(item: DataItem, selected: boolean): void {
        if (this.type !== 'multiselect') {
            return;
        }
        const index = this.selectedDataItems.findIndex(selectedItem => selectedItem.id === item.id);
        if (selected && index === -1) {
            this.selectedDataItems.push(item);
        } else if (!selected && index !== -1) {
            this.selectedDataItems.splice(index, 1);
        }

        if (item.children) {
            item.children.forEach(child => {
                const childIndex = this.selectedDataItems.findIndex(selectedItem => selectedItem.id === child.id);
                if (selected && childIndex === -1) {
                    this.selectedDataItems.push(child);
                } else if (!selected && childIndex !== -1) {
                    this.selectedDataItems.splice(childIndex, 1);
                }
                this.toggleChildrenSelection(child, selected);
            });
        }
        this.formGroup?.get(this.controlName)?.setValue(this.selectedDataItems.map(x => x.id));
    }

    public showDebugInfo(): void {
        console.log(this.formGroup);
        console.log(this.currentControl);
        console.log(this.currentValue);
        console.log(this.dataItems)
    }
}
