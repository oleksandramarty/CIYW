import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Observable, take, takeUntil, tap} from "rxjs";
import {
    FilteredListResponseOfPlannedExpenseResponse,
    PlannedExpenseResponse,
    UserProjectResponse
} from "../../../../../core/api-models/common.models";
import {FormControl} from "@angular/forms";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Store} from "@ngrx/store";
import {LoaderService} from "../../../../../core/services/loader.service";
import {GraphQlExpensesService} from "../../../../../core/graph-ql/services/graph-ql-expenses.service";
import {CommonDialogService} from "../../../../../core/services/common-dialog.service";
import {handleApiError} from "../../../../../core/helpers/rxjs.helper";
import {BaseGraphQlFilteredModel} from "../../../../../core/models/common/base-graphql.model";
import {selectPlannedExpensesSnapshot} from "../../../../../core/store/selectors/expenses.selectors";
import {expenses_setUserProject_plannedExpensesSnapshot} from "../../../../../core/store/actions/expenses.actions";
import {BaseFilterComponent} from "../../../../../core/base-components/base-filter.component";
import {LocalizationService} from "../../../../../core/services/localization.service";
import {map} from "rxjs/operators";
import {createUserProjectPlannedExpensesHeader, ITableHeaderItem} from "../../../../../core/models/table.model";

@Component({
    selector: 'app-user-project-planned-expenses',
    templateUrl: './user-project-planned-expenses.component.html',
    styleUrl: '../user-project.component.scss'
})
export class UserProjectPlannedExpensesComponent extends BaseFilterComponent<FilteredListResponseOfPlannedExpenseResponse, [BaseGraphQlFilteredModel, string, number[]]> {
    @Input() userProject: UserProjectResponse | undefined;
    @Output() plannedExpenseChanged: EventEmitter<void> = new EventEmitter();

    public tableHeaderItems: ITableHeaderItem[] = createUserProjectPlannedExpensesHeader();

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

        this.initializeFilterForm(this.store.select(selectPlannedExpensesSnapshot));

        this.store.select(selectPlannedExpensesSnapshot)
            .pipe(
                take(1),
                tap((result) => {
                    if (result && result.categoryIds) {
                        this.filterFormGroup.get('categoryIds')?.setValue(result.categoryIds);
                    } else {
                        this.getFilteredItems();
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    override ngOnDestroy(): void {
        this.store.dispatch(expenses_setUserProject_plannedExpensesSnapshot({
            filteredResult: this.filteredResult!,
            paginator: this.paginator,
            sort: this.sort,
            dateRange: this.filterFormGroup.value.dateRange,
            query: this.filterFormGroup.value.query,
            categoryIds: this.filterFormGroup.value.categoryIds
        }));
        super.ngOnDestroy();
    }

    public openCreateUpdatePlannedExpenseDialog(plannedExpense: PlannedExpenseResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdatePlannedExpenseDialog(() => {
            this.getFilteredItems();
            this.plannedExpenseChanged.emit();
        }, () => {
        }, plannedExpense, this.userProject);
    }

    public removePlannedExpense(plannedExpense: PlannedExpenseResponse): void {
        const removePlannedExpenseAction = () => {
            const removePlannedExpenseActionProceed = () => {
                this.graphQlExpensesService.removePlannedExpense(plannedExpense.id!)
                    .pipe(
                        takeUntil(this.ngUnsubscribe),
                        tap((result) => {
                            this.getFilteredItems();
                        }),
                        handleApiError(this.snackBar)
                    )
                    .subscribe();
            }
            this.commonDialogService.showRemoveExpenseConfirmationDialog(removePlannedExpenseActionProceed, () => {
            });
        }
        this.commonDialogService.showNoComplaintDialog(removePlannedExpenseAction, () => {
        });
    }

    protected createFilterParams(): [BaseGraphQlFilteredModel, string, number[]] {
        return [
            this.filterBaseModel,
            this.userProject!.id,
            this.filterFormGroup?.value.categoryIds?.map(Number) ?? []
        ];
    }

    protected getFilteredItemsSub(filterRequest: [BaseGraphQlFilteredModel, string, number[]]): Observable<FilteredListResponseOfPlannedExpenseResponse> {
        return this.graphQlExpensesService.getFilteredPlannedExpenses(...filterRequest).pipe(
            map(result => result.data.expenses_get_filtered_planned_expenses!)
        );
    }
}
