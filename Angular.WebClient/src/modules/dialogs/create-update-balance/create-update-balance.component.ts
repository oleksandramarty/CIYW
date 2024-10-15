import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {MatButton, MatButtonModule} from "@angular/material/button";
import {
    MAT_DIALOG_DATA,
    MatDialogActions,
    MatDialogModule,
    MatDialogRef,
    MatDialogTitle
} from "@angular/material/dialog";
import {CommonModule, NgIf} from "@angular/common";
import {Subject, takeUntil, tap} from "rxjs";
import {BalanceResponse, IconResponse, UserProjectResponse} from "../../../core/api-models/common.models";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "../../../core/services/localization.service";
import {GraphQlExpensesService} from "../../../core/graph-ql/services/graph-ql-expenses.service";
import {LoaderService} from "../../../core/services/loader.service";
import {CommonDialogService} from "../../../core/services/common-dialog.service";
import {handleApiError} from "../../../core/helpers/rxjs.helper";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../../core/shared.module";
import {DataItem} from "../../../core/models/common/data-item.model";
import {DictionaryService} from "../../../core/services/dictionary.service";
import {InputIconPickerComponent} from "../../common/common-input/input-icon-picker/input-icon-picker.component";

@Component({
  selector: 'app-create-update-balance',
    standalone: true,
    imports: [
        CommonModule,
        MatDialogModule,
        MatButtonModule,
        CommonLoaderComponent,
        AppCommonInputModule,
        InputIconPickerComponent,
        RouterLink,
        SharedModule,
    ],
  templateUrl: './create-update-balance.component.html',
  styleUrl: './create-update-balance.component.scss'
})
export class CreateUpdateBalanceComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();

    public balance: BalanceResponse | undefined;
    public userProject: UserProjectResponse | undefined;
    public balanceFormGroup: FormGroup | undefined;

    get currencies(): DataItem[] | undefined {
        return this.dictionaryService.dataItems?.currencies;
    }

    get balanceTypes(): DataItem[] | undefined {
        return this.dictionaryService.dataItems?.balanceTypes;
    }

    constructor(
        public dialogRef: MatDialogRef<CreateUpdateBalanceComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            balance: BalanceResponse | undefined,
            userProject: UserProjectResponse | undefined
        },
        private readonly snackBar: MatSnackBar,
        private readonly fb: FormBuilder,
        private readonly localizationService: LocalizationService,
        private readonly graphQlExpensesService: GraphQlExpensesService,
        private readonly loaderService: LoaderService,
        private readonly commonDialogService: CommonDialogService,
        private readonly dictionaryService: DictionaryService
    ) {
        this.balance = data.balance;
        this.userProject = data.userProject;
    }

    ngOnInit(): void {
        this.createUserForm();
    }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    private createUserForm() {
        this.balanceFormGroup = this.fb.group({
            title: [this.balance?.title, [Validators.required]],
            isActive: [this.balance?.isActive ?? true, [Validators.required]],
            currencyId: [this.balance?.currencyId, [Validators.required]],
            balanceTypeId: [this.balance?.balanceTypeId, [Validators.required]],
            iconId: [this.balance?.iconId, [Validators.required]],
        });
    }

    public createOrUpdateBalance() {
        if (!this.balanceFormGroup?.valid) {
            Object.keys(this.balanceFormGroup?.controls ?? {}).forEach(key => {
                const control = this.balanceFormGroup?.get(key);
                if (control) {
                    control.markAsTouched();
                }
            });
            this.snackBar.open(this.localizationService?.getTranslation('ERROR.FORM_VALIDATION') ?? 'ERROR', 'Close', { duration: 3000 });
            return;
        }

        const createOrUpdateBalanceAction = () => {
            if (!this.balanceFormGroup) {
                return;
            }

            this.loaderService.isBusy = true;

            (!!this.balance ?
                this.graphQlExpensesService.updateUserBalance(
                    this.balance.id,
                    ...this.inputParams)  :
                this.graphQlExpensesService.createUserBalance(
                    ...this.inputParams))
                .pipe(
                    takeUntil(this.ngUnsubscribe),
                    tap(() => {
                        this.snackBar.open(this.localizationService?.getTranslation('SUCCESS') ?? '', 'Close', { duration: 3000 });
                        this.loaderService.isBusy = false;
                        this.dialogRef.close(true);
                    }),
                    handleApiError(this.snackBar)
                ).subscribe();
        }

        this.commonDialogService.showNoComplaintModal(createOrUpdateBalanceAction)
    }

    get inputParams(): [string, boolean, number, number, string, number] {
        return [
            this.balanceFormGroup?.value.title,
            this.balanceFormGroup?.value.isActive,
            Number(this.balanceFormGroup?.value.currencyId),
            Number(this.balanceFormGroup?.value.balanceTypeId),
            this.userProject!.id,
            this.balanceFormGroup?.value.iconId
        ];
    }
}
