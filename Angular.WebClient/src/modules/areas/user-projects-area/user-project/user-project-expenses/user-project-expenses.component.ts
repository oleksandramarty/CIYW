import {Component, EventEmitter, Input, Output} from '@angular/core';
import {FormControl} from '@angular/forms';
import {MatSnackBar} from '@angular/material/snack-bar';
import {Store} from '@ngrx/store';
import {Observable, take, takeUntil, tap} from 'rxjs';
import {
    ExpenseResponse,
    FilteredListResponseOfExpenseResponse,
    UserProjectResponse
} from '../../../../../core/api-models/common.models';
import {DictionaryService} from '../../../../../core/services/dictionary.service';
import {LoaderService} from '../../../../../core/services/loader.service';
import {GraphQlExpensesService} from '../../../../../core/graph-ql/services/graph-ql-expenses.service';
import {CommonDialogService} from '../../../../../core/services/common-dialog.service';
import {handleApiError} from '../../../../../core/helpers/rxjs.helper';
import {BaseGraphQlFilteredModel} from '../../../../../core/models/common/base-graphql.model';
import {selectExpensesSnapshot} from '../../../../../core/store/selectors/expenses.selectors';
import {expenses_setUserProject_expensesSnapshot} from '../../../../../core/store/actions/expenses.actions';
import {BaseFilterComponent} from '../../../../../core/base-components/base-filter.component';
import {LocalizationService} from '../../../../../core/services/localization.service';
import {map} from "rxjs/operators";
import {createUserProjectExpensesHeader, ITableHeaderItem} from "../../../../../core/models/table.model";

@Component({
    selector: 'app-user-project-expenses',
    templateUrl: './user-project-expenses.component.html',
    styleUrls: ['../user-project.component.scss']
})
export class UserProjectExpensesComponent extends BaseFilterComponent<FilteredListResponseOfExpenseResponse, [BaseGraphQlFilteredModel, string, number[]]> {
    @Input() userProject: UserProjectResponse | undefined;
    @Output() expanseChanged: EventEmitter<void> = new EventEmitter();

    public tableHeaderItems: ITableHeaderItem[] = createUserProjectExpensesHeader();

    constructor(
        protected override readonly dictionaryService: DictionaryService,
        protected override readonly localizationService: LocalizationService,
        protected override readonly loaderService: LoaderService,
        protected override readonly snackBar: MatSnackBar,
        private readonly store: Store,
        private readonly graphQlExpensesService: GraphQlExpensesService,
        private readonly commonDialogService: CommonDialogService
    ) {
        super(dictionaryService, localizationService, loaderService, snackBar);
    }

    override ngOnInit(): void {
        this.filterFormGroup.addControl('categoryIds', new FormControl([]));

        this.initializeFilterForm(this.store.select(selectExpensesSnapshot));

        this.store.select(selectExpensesSnapshot)
            .pipe(
                take(1),
                tap((result) => {
                    if (result && result.categoryIds) {
                        this.filterFormGroup.get('categoryIds')?.setValue(result.categoryIds);
                    } else {
                        this.filteredItems();
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    override ngOnDestroy(): void {
        this.store.dispatch(expenses_setUserProject_expensesSnapshot({
            filteredResult: this.filteredResult!,
            paginator: this.paginator,
            sort: this.sort,
            dateRange: this.filterFormGroup.value.dateRange,
            query: this.filterFormGroup.value.query,
            categoryIds: this.filterFormGroup.value.categoryIds
        }));
        super.ngOnDestroy();
    }

    public openCreateUpdateExpenseDialog(expense: ExpenseResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdateExpenseDialog(() => {
            this.filteredItems();
            this.expanseChanged.emit();
        }, () => {
        }, expense, this.userProject);
    }

    public removeExpense(expense: ExpenseResponse): void {
        const removeExpenseAction = () => {
            const removeExpenseActionProceed = () => {
                this.graphQlExpensesService.removeExpense(expense.id!)
                    .pipe(
                        takeUntil(this.ngUnsubscribe),
                        tap(() => {
                            this.filteredItems();
                            this.expanseChanged.emit();
                        }),
                        handleApiError(this.snackBar)
                    )
                    .subscribe();
            }
            this.commonDialogService.showRemoveExpenseConfirmationDialog(removeExpenseActionProceed, () => {
            });
        }
        this.commonDialogService.showNoComplaintDialog(removeExpenseAction, () => {
        });
    }

    protected createFilterParams(): [BaseGraphQlFilteredModel, string, number[]] {
        return [
            this.filterBaseModel,
            this.userProject!.id,
            this.filterFormGroup?.value.categoryIds?.map(Number) ?? []
        ];
    }

    protected filteredItemsSub(filterRequest: [BaseGraphQlFilteredModel, string, number[]]): Observable<FilteredListResponseOfExpenseResponse> {
        return this.graphQlExpensesService.filteredExpenses(...filterRequest).pipe(
            map(result => result.data.expenses_filtered_expenses!)
        );
    }
}