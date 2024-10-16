import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {CommonModule} from "@angular/common";
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {InputIconPickerComponent} from "../../common/common-input/input-icon-picker/input-icon-picker.component";
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../../core/shared.module";
import {Subject, takeUntil, tap} from "rxjs";
import {FavoriteExpenseResponse, UserProjectResponse} from "../../../core/api-models/common.models";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {DataItem} from "../../../core/models/common/data-item.model";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "../../../core/services/localization.service";
import {GraphQlExpensesService} from "../../../core/graph-ql/services/graph-ql-expenses.service";
import {LoaderService} from "../../../core/services/loader.service";
import {CommonDialogService} from "../../../core/services/common-dialog.service";
import {DictionaryService} from "../../../core/services/dictionary.service";
import {handleApiError} from "../../../core/helpers/rxjs.helper";
import {handleIntId} from "../../../core/helpers/apollo.helper";

@Component({
    selector: 'app-create-update-favorite-expense',
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
    templateUrl: './create-update-favorite-expense.component.html',
    styleUrl: './create-update-favorite-expense.component.scss'
})
export class CreateUpdateFavoriteExpenseComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();

    public favoriteExpense: FavoriteExpenseResponse | undefined;
    public userProject: UserProjectResponse | undefined;
    public favoriteExpenseFormGroup: FormGroup | undefined;

    get currencies(): DataItem[] | undefined {
        return this.dictionaryService.dataItems?.currencies;
    }

    get categories(): DataItem[] | undefined {
        return this.dictionaryService.dataItems?.categoriesFlat;
    }

    get frequencies(): DataItem[] | undefined {
        return this.dictionaryService.dataItems?.frequencies;
    }

    constructor(
        public dialogRef: MatDialogRef<CreateUpdateFavoriteExpenseComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {
            favoriteExpense: FavoriteExpenseResponse | undefined,
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
        this.favoriteExpense = data.favoriteExpense;
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
        this.favoriteExpenseFormGroup = this.fb.group({
            title: [this.favoriteExpense?.title, [Validators.required, Validators.maxLength(50)]],
            description: [this.favoriteExpense?.description, [Validators.maxLength(255)]],
            limit: [!!this.favoriteExpense && (this.favoriteExpense!.limit ?? 0) > 0 ? this.favoriteExpense?.limit : null, [Validators.min(0)]],
            categoryId: [this.favoriteExpense?.categoryId],
            frequencyId: [this.favoriteExpense?.frequencyId],
            currencyId: [this.favoriteExpense?.currencyId, [Validators.required]],
            iconId: [this.favoriteExpense?.iconId, [Validators.required]],
        });
    }

    public removeFavoriteExpense(): void {
        this.snackBar.open('In development', 'Close', { duration: 3000 });
    }

    public createOrUpdateFavoriteExpense() {
        if (!this.favoriteExpenseFormGroup?.valid) {
            Object.keys(this.favoriteExpenseFormGroup?.controls ?? {}).forEach(key => {
                const control = this.favoriteExpenseFormGroup?.get(key);
                if (control) {
                    control.markAsTouched();
                }
            });
            this.snackBar.open(this.localizationService?.getTranslation('ERROR.FORM_VALIDATION') ?? 'ERROR', 'Close', {duration: 3000});
            return;
        }

        const createOrUpdateBalanceAction = () => {
            if (!this.favoriteExpenseFormGroup) {
                return;
            }

            this.loaderService.isBusy = true;

            (!!this.favoriteExpense ?
                this.graphQlExpensesService.updateFavoriteExpense(
                    this.favoriteExpense.id,
                    ...this.inputParams) :
                this.graphQlExpensesService.createFavoriteExpense(
                    ...this.inputParams))
                .pipe(
                    takeUntil(this.ngUnsubscribe),
                    tap(() => {
                        this.snackBar.open(this.localizationService?.getTranslation('SUCCESS') ?? '', 'Close', {duration: 3000});
                        this.loaderService.isBusy = false;
                        this.dialogRef.close(true);
                    }),
                    handleApiError(this.snackBar)
                ).subscribe();
        }

        this.commonDialogService.showNoComplaintModal(createOrUpdateBalanceAction)
    }

    get inputParams(): [
        string,
            string | undefined,
            number | undefined,
            number | undefined,
            number | undefined,
        number,
        string,
        number
    ] {
        return [
            this.favoriteExpenseFormGroup?.value.title,
            this.favoriteExpenseFormGroup?.value.description,
            handleIntId(this.favoriteExpenseFormGroup?.value.limit),
            handleIntId(this.favoriteExpenseFormGroup?.value.categoryId),
            handleIntId(this.favoriteExpenseFormGroup?.value.frequencyId),
            Number(this.favoriteExpenseFormGroup?.value.currencyId),
            this.userProject!.id,
            Number(this.favoriteExpenseFormGroup?.value.iconId)
        ];
    }
}
