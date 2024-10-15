import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {CommonModule} from "@angular/common";
import {MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {CommonLoaderComponent} from "../../common-loader/common-loader.component";
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../../../core/shared.module";
import {FormGroup, Validators} from "@angular/forms";
import {Subject, takeUntil, tap} from "rxjs";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {CommonDialogService} from "../../../../core/services/common-dialog.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {DictionaryMap} from "../../../../core/models/common/dictionary.model";
import {IconResponse} from "../../../../core/api-models/common.models";

@Component({
    selector: 'app-input-icon-picker',
    standalone: true,
    imports: [
        CommonModule,
        MatDialogModule,
        MatButtonModule,
        CommonLoaderComponent,
        RouterLink,
        SharedModule,
    ],
    templateUrl: './input-icon-picker.component.html',
    styleUrl: './input-icon-picker.component.scss'
})
export class InputIconPickerComponent implements OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    @Input() formGroup: FormGroup | undefined;
    @Input() controlName: string = 'inputControl';

    get icon(): string | undefined {
        return this.dictionaryService.iconMap?.get(this.formGroup?.get(this.controlName)?.value)?.title;
    }

    get isRequired(): boolean {
        return this.formGroup?.get(this.controlName)?.hasValidator(Validators.required) ?? false;
    }

    constructor(
        private readonly dictionaryService: DictionaryService,
        private readonly commonDialogService: CommonDialogService,
        private readonly snackBar: MatSnackBar,
    ) {
    }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public openIconPicker(): void {
        this.commonDialogService.showIconPickerModal()
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap((icon: number | undefined) => {
                    this.formGroup?.get(this.controlName)?.setValue(icon);
                }),
                handleApiError(this.snackBar),
            ).subscribe();
    }

    public removeIcon(): void {
        this.formGroup?.get(this.controlName)?.setValue(undefined);
    }
}
