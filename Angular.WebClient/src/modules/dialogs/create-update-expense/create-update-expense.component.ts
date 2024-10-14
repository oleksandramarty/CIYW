import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { CommonModule, CurrencyPipe } from "@angular/common";
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";
import { CommonLoaderComponent } from "../../common/common-loader/common-loader.component";
import { AppCommonInputModule } from "../../common/common-input/app-common-input.module";
import { RouterLink } from "@angular/router";
import { Subject, takeUntil, tap } from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { DictionaryService } from "../../../core/services/dictionary.service";
import { DictionaryDataItems, DictionaryMap } from "../../../core/models/common/dictionary.model";
import { DataItem } from "../../../core/models/common/data-item.model";
import { LocalizationService } from "../../../core/services/localization.service";
import { provideNativeDateAdapter } from "@angular/material/core";
import { handleApiError } from "../../../core/helpers/rxjs.helper";
import { SharedModule } from "../../../core/shared.module";
import {LoaderService} from "../../../core/services/loader.service";
import {CommonDialogService} from "../../../core/services/common-dialog.service";
import {GraphQlExpensesService} from "../../../core/graph-ql/services/graph-ql-expenses.service";
import {
    BalanceResponse,
    CategoryResponse,
    CurrencyResponse,
    ExpenseResponse,
    UserProjectResponse
} from "../../../core/api-models/common.models";

@Component({
    selector: 'app-create-update-expense',
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
    templateUrl: './create-update-expense.component.html',
    styleUrls: ['./create-update-expense.component.scss'] // Corrected property name
})
export class CreateUpdateExpenseComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    public expenseFormGroup: FormGroup | undefined;

    public expense: ExpenseResponse | undefined;
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
        return this.balancesDataItems?.find(balance => balance.id === this.expenseFormGroup?.value.balanceId);
    }

    get isPositive(): boolean {
        return this.dictionaryService.categoriesMap?.get(Number(this.expenseFormGroup?.value.categoryId))?.isPositive ?? false;
    }

    get predictedBalance(): number {
        let resultAmount = this.expenseFormGroup?.value.amount - (this.expense?.amount ?? 0);
        return (this.currentBalance?.originalValue as BalanceResponse)?.amount + (this.isPositive ? resultAmount : -resultAmount);
    }

    constructor(
        public dialogRef: MatDialogRef<CreateUpdateExpenseComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            expense: ExpenseResponse | undefined,
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
        this.expense = data?.expense;
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
        this.createExpenseForm();
    }

    ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public createExpenseForm(): void {
        this.expenseFormGroup = this.fb.group({
            title: [this.expense?.title, [Validators.required, Validators.maxLength(100)]],
            description: [this.expense?.description, Validators.maxLength(300)],
            amount: [this.expense?.amount, [Validators.required, Validators.min(0)]],
            date: [this.expense?.date ?? new Date(), Validators.required],
            balanceId: [this.expense?.balanceId ?? this.balancesDataItems![0]?.id, Validators.required],
            categoryId: [String(this.expense?.categoryId), Validators.required]
        });
    }

    public createOrUpdateExpense(): void {
        if (!this.expenseFormGroup?.valid) {
            Object.keys(this.expenseFormGroup?.controls ?? {}).forEach(key => {
                const control = this.expenseFormGroup?.get(key);
                if (control) {
                    control.markAsTouched();
                }
            });
            this.snackBar.open(this.localizationService?.getTranslation('ERROR.FORM_VALIDATION') ?? 'ERROR', 'Close', { duration: 3000 });
            return;
        }

        const createOrUpdateExpenseAction = () => {
            if (!this.expenseFormGroup) {
                return;
            }

            this.loaderService.isBusy = true;

            this.graphQlExpensesService.createOrUpdateExpense(
                this.expense?.id,
                this.expenseFormGroup.value.title,
                this.expenseFormGroup.value.description,
                Number(this.expenseFormGroup.value.amount),
                this.expenseFormGroup.value.balanceId,
                this.expenseFormGroup.value.date,
                Number(this.expenseFormGroup.value.categoryId),
                !this.expense ? this.userProject?.id : undefined
            ).pipe(
                takeUntil(this.ngUnsubscribe),
                tap(() => {
                    this.snackBar.open(this.localizationService?.getTranslation('SUCCESS.EXPENSE_CREATED') ?? 'SUCCESS', 'Close', { duration: 3000 });
                    this.loaderService.isBusy = false;
                    this.dialogRef.close(true);
                }),
                handleApiError(this.snackBar, this.localizationService)
            ).subscribe();
        }

        this.commonDialogService.showNoComplaintModal(createOrUpdateExpenseAction);
    }
}