import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {CommonModule, CurrencyPipe} from "@angular/common";
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../../core/shared.module";
import {provideNativeDateAdapter} from "@angular/material/core";
import {Subject, takeUntil, tap} from "rxjs";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {DataItem} from "../../../core/models/common/data-item.model";
import {DictionaryDataItems, DictionaryMap} from "../../../core/models/common/dictionary.model";
import {MatSnackBar} from "@angular/material/snack-bar";
import {DictionaryService} from "../../../core/services/dictionary.service";
import {LocalizationService} from "../../../core/services/localization.service";
import {LoaderService} from "../../../core/services/loader.service";
import {CommonDialogService} from "../../../core/services/common-dialog.service";
import {GraphQlExpensesService} from "../../../core/graph-ql/services/graph-ql-expenses.service";
import {handleApiError} from "../../../core/helpers/rxjs.helper";
import {
    CategoryResponse,
    CurrencyResponse,
    PlannedExpenseResponse,
    UserProjectResponse
} from "../../../core/api-models/common.models";

@Component({
    selector: 'app-create-update-planned-expense',
    standalone: true,
    imports: [
        CommonModule,
        MatDialogModule,
        MatButtonModule,
        CommonLoaderComponent,
        AppCommonInputModule,
        RouterLink,
        SharedModule,
    ],
    providers: [
        CurrencyPipe,
        provideNativeDateAdapter()
    ],
    templateUrl: './create-update-planned-expense.component.html',
    styleUrl: './create-update-planned-expense.component.scss'
})
export class CreateUpdatePlannedExpenseComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    public plannedExpenseFormGroup: FormGroup | undefined;

    public plannedExpense: PlannedExpenseResponse | undefined;
    public userProject: UserProjectResponse | undefined;

    public balancesDataItems: DataItem[] | undefined;

    get dataItems(): DictionaryDataItems | undefined {
        return this.dictionaryService.dataItems;
    }

    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        return this.dictionaryService.currenciesMap;
    }

    get categoriesMap(): DictionaryMap<number, CategoryResponse> | undefined {
        return this.dictionaryService.categoriesMap;
    }

    get currentBalance(): DataItem | undefined {
        return this.balancesDataItems?.find(balance => balance.id === this.plannedExpenseFormGroup?.value.balanceId);
    }

    get isPositive(): boolean {
        return this.dictionaryService.categoriesMap?.get(Number(this.plannedExpenseFormGroup?.value.categoryId))?.isPositive ?? false;
    }

    constructor(
        public dialogRef: MatDialogRef<CreateUpdatePlannedExpenseComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            plannedExpense: PlannedExpenseResponse | undefined,
            userProject: UserProjectResponse | undefined
        } | undefined,
        private readonly snackBar: MatSnackBar,
        private readonly fb: FormBuilder,
        private readonly dictionaryService: DictionaryService,
        private readonly localizationService: LocalizationService,
        private readonly currencyPipe: CurrencyPipe,
        private readonly loaderService: LoaderService,
        private readonly commonDialogService: CommonDialogService,
        private readonly graphQlExpensesService: GraphQlExpensesService
    ) {
        this.plannedExpense = data?.plannedExpense;
        this.userProject = data?.userProject;

        this.balancesDataItems = this.userProject?.balances?.map(
            balance =>
                new DataItem(
                    balance,
                    balance.id,
                    `${this.currenciesMap?.get(balance.currencyId)?.code} ${this.currencyPipe.transform(balance.amount, this.currenciesMap?.get(balance.currencyId)?.code)}`,
                    this.currenciesMap?.get(balance.currencyId)?.title));
    }

    ngOnInit(): void {
        this.createPlannedExpenseForm();
    }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public createPlannedExpenseForm(): void {
        this.plannedExpenseFormGroup = this.fb.group({
            title: [this.plannedExpense?.title, [Validators.required, Validators.maxLength(100)]],
            description: [this.plannedExpense?.description, Validators.maxLength(300)],
            amount: [this.plannedExpense?.amount, [Validators.required, Validators.min(0)]],
            startDate: [this.plannedExpense?.startDate ?? new Date(), Validators.required],
            endDate: [this.plannedExpense?.endDate ?? null],
            balanceId: [this.plannedExpense?.balanceId ?? this.balancesDataItems![0]?.id, Validators.required],
            categoryId: [String(this.plannedExpense?.categoryId), Validators.required],
            isActive: [this.plannedExpense?.isActive ?? true, Validators.required],
            frequencyId: [this.plannedExpense?.frequencyId ?? null, Validators.required]
        });
    }

    public createOrUpdateExpense(): void {
        if (!this.plannedExpenseFormGroup?.valid) {
            Object.keys(this.plannedExpenseFormGroup?.controls ?? {}).forEach(key => {
                const control = this.plannedExpenseFormGroup?.get(key);
                if (control) {
                    control.markAsTouched();
                }
            });
            this.snackBar.open(this.localizationService?.getTranslation('ERROR.FORM_VALIDATION') ?? 'ERROR', 'Close', {duration: 3000});
            return;
        }

        const createOrUpdatePlannedExpenseAction = () => {
            if (!this.plannedExpenseFormGroup) {
                return;
            }

            this.loaderService.isBusy = true;

            this.graphQlExpensesService.createOrUpdatePlannedExpense(
                this.plannedExpense?.id,
                this.plannedExpenseFormGroup.value.title,
                this.plannedExpenseFormGroup.value.description,
                Number(this.plannedExpenseFormGroup.value.amount),
                this.plannedExpenseFormGroup.value.balanceId,
                this.plannedExpenseFormGroup.value.startDate,
                this.plannedExpenseFormGroup.value.endDate,
                Number(this.plannedExpenseFormGroup.value.categoryId),
                !this.plannedExpense ? this.userProject?.id : undefined,
                Number(this.plannedExpenseFormGroup.value.frequencyId),
                this.plannedExpenseFormGroup.value.isActive
            ).pipe(
                takeUntil(this.ngUnsubscribe),
                tap(() => {
                    this.snackBar.open(this.localizationService?.getTranslation('SUCCESS.EXPENSE_CREATED') ?? 'SUCCESS', 'Close', {duration: 3000});
                    this.loaderService.isBusy = false;
                    this.dialogRef.close(true);
                }),
                handleApiError(this.snackBar, this.localizationService)
            ).subscribe();
        }

        this.commonDialogService.showNoComplaintModal(createOrUpdatePlannedExpenseAction);
    }
}
