import {Component, OnDestroy, OnInit} from '@angular/core';
import {finalize, Subject, take, takeUntil, tap} from "rxjs";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {selectExpensesState} from "../../../../core/store/selectors/expenses.selectors";
import {Store} from "@ngrx/store";
import {DictionaryMap} from "../../../../core/models/common/dictionarie.model";
import {FormControl, FormGroup} from "@angular/forms";
import {DataItem} from "../../../../core/models/common/data-item.model";
import {getUTCString, handleBaseDateRangeFilter} from "../../../../core/helpers/date-time.helper";
import {LoaderService} from "../../../../core/services/loader.service";
import {GraphQlExpensesService} from "../../../../core/graph-ql/services/graph-ql-expenses.service";
import {CategoryResponse, CurrencyResponse, FrequencyResponse} from "../../../../core/api-clients/dictionaries-client";
import {
    BaseSortableRequest,
    ColumnEnum,
    ExpenseResponse,
    FilteredListResponseOfExpenseResponse,
    FilteredListResponseOfPlannedExpenseResponse,
    OrderDirectionEnum,
    PaginatorEntity, PlannedExpenseResponse,
    UserProjectResponse
} from "../../../../core/api-clients/common-module.client";
import {CommonDialogService} from "../../../../core/services/common-dialog.service";
import {fadeInOut} from "../../../../core/animations/animations";
import {BaseGraphQlFilteredModel} from "../../../../core/models/common/base-graphql.model";

@Component({
    selector: 'app-user-project',
    templateUrl: './user-project.component.html',
    styleUrl: './user-project.component.scss',
    animations: [fadeInOut]
})
export class UserProjectComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    userProjectId: string | undefined;
    userProject: UserProjectResponse | undefined;
    expenses: FilteredListResponseOfExpenseResponse | undefined;
    plannedExpenses: FilteredListResponseOfPlannedExpenseResponse | undefined;
    paginatorExpenses: PaginatorEntity = new PaginatorEntity({pageSize: 10, pageNumber: 0, isFull: false});
    paginatorPlannedExpenses: PaginatorEntity = new PaginatorEntity({pageSize: 10, pageNumber: 0, isFull: false});
    sortExpenses: BaseSortableRequest = new BaseSortableRequest({column: ColumnEnum.Date, direction: OrderDirectionEnum.Desc});
    sortPlannedExpenses: BaseSortableRequest = new BaseSortableRequest({column: ColumnEnum.NextDate, direction: OrderDirectionEnum.Desc});

    activeTab: number = 0;

    public utcTimeShift: string = getUTCString();

    filterFormGroupExpenses: FormGroup = new FormGroup({
        dateRange: new FormControl(),
        query: new FormControl(),
        categoryIds: new FormControl(),
    });
    filterFormGroupPlannedExpenses: FormGroup = new FormGroup({
        dateRange: new FormControl(),
        query: new FormControl(),
        categoryIds: new FormControl(),
    });

    get sort(): BaseSortableRequest {
        return this.activeTab === 0 ? this.sortExpenses : this.sortPlannedExpenses;
    }

    get filterFormGroup(): FormGroup {
        return this.activeTab === 0 ? this.filterFormGroupExpenses : this.filterFormGroupPlannedExpenses;
    }

    get paginator(): PaginatorEntity {
        return this.activeTab === 0 ? this.paginatorExpenses : this.paginatorPlannedExpenses;
    }

    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        return this.dictionaryService.currenciesMap;
    }

    get frequenciesMap(): DictionaryMap<number, FrequencyResponse> | undefined {
        return this.dictionaryService.frequenciesMap;
    }

    get categoriesMap(): DictionaryMap<number, CategoryResponse> | undefined {
        return this.dictionaryService.categoriesMap;
    }

    get categoriesDataItems(): DataItem[] {
        return this.dictionaryService.dataItems?.categories ?? [];
    }

    constructor(
        private readonly dictionaryService: DictionaryService,
        private snackBar: MatSnackBar,
        private route: ActivatedRoute,
        private router: Router,
        private readonly store: Store,
        private readonly loaderService: LoaderService,
        private readonly graphQlExpensesService: GraphQlExpensesService,
        private readonly commonDialogService: CommonDialogService
    ) {
        this.route.paramMap
            .pipe(
                take(1),
                tap((params: ParamMap) => {
                    const id = params.get('id');
                    if (id) {
                        this.userProjectId = String(id);
                        this.getUserProject();
                    } else {
                        this.router.navigate(['/projects']);
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    public ngOnInit(): void {
    }

    public ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public openCreateUpdateExpenseDialog(expense: ExpenseResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdateExpenseModal(() => {
            this.getFilteredItems();
            this.getUserProjectFromApi();
        }, expense, this.userProject);
    }

    public openCreateUpdatePlannedExpenseDialog(plannedExpense: PlannedExpenseResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdatePlannedExpenseModal(() => {
            this.getFilteredItems();
            this.getUserProjectFromApi();
        }, plannedExpense, this.userProject);
    }

    public removeExpense(expense: ExpenseResponse): void {
        const removeExpenseAction = () => {
            const removeExpenseActionProceed = () => {
                this.graphQlExpensesService.removeExpense(expense.id!)
                    .pipe(
                        takeUntil(this.ngUnsubscribe),
                        tap((result) => {
                            this.getFilteredItems();
                            this.getUserProjectFromApi();
                        }),
                        handleApiError(this.snackBar)
                    )
                    .subscribe();
            }

            this.commonDialogService.showRemoveExpenseConfirmationModal(removeExpenseActionProceed);
        }

        this.commonDialogService.showNoComplaintModal(removeExpenseAction);
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

            this.commonDialogService.showRemoveExpenseConfirmationModal(removePlannedExpenseActionProceed);
        }

        this.commonDialogService.showNoComplaintModal(removePlannedExpenseAction);
    }

    private getUserProject(): void {
        this.store.select(selectExpensesState)
            .pipe(
                take(1),
                tap(expensesState => {
                    if (!!expensesState.userProjects && !!expensesState.userAllowedProjects) {
                        const userProject = expensesState.userProjects?.entities?.find(project => project.id === this.userProjectId);
                        const userAllowedProject = expensesState.userProjects?.entities?.find(project => project.id === this.userProjectId);
                        if (!!userProject || !!userAllowedProject) {
                            this.userProject = userProject ?? userAllowedProject;
                            this.getFilteredItems();
                        } else {
                            this.getUserProjectFromApi();
                        }
                    } else {
                        this.getUserProjectFromApi();
                    }
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    private getUserProjectFromApi(): void {
        this.loaderService.isBusy = true;
        this.graphQlExpensesService.getUserProjectById(this.userProjectId!)
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap(result => {
                    const userProject = result?.data?.expenses_get_user_project_by_id as UserProjectResponse;
                    this.userProject = userProject;
                    this.getFilteredItems();
                    this.loaderService.isBusy = false;
                }),
                handleApiError(this.snackBar),
                finalize(() => this.loaderService.isBusy = false)
            )
            .subscribe();
    }

    public resetFilter(): void {
        this.filterFormGroup.reset();
        this.getFilteredItems();
    }

    get filterParams(): [BaseGraphQlFilteredModel, string, number[]] {
        const dateRange = handleBaseDateRangeFilter(this.filterFormGroup!.value?.dateRange);

        return [
            {
                dateFrom: dateRange?.startDate,
                dateTo: dateRange?.endDate,
                amountFrom: undefined,
                amountTo: undefined,
                isFull: this.paginator?.isFull ?? false,
                pageNumber: this.paginator?.pageNumber ?? 1,
                pageSize: this.paginator?.pageSize ?? 10,
                column: this.sort?.column?.toString() ?? (this.activeTab === 0 ? 'Date' : 'NextDate'),
                direction: this.sort?.direction?.toString() ?? 'Desc',
                query: this.filterFormGroup?.value?.query ?? '',
            },
            this.userProjectId!,
            this.filterFormGroup?.value.categoryIds?.split(',').map(Number) ?? []
        ];
    }

    public getFilteredItems(): void {
        this.loaderService.isBusy = true;

        if (this.activeTab === 0) {
            this.graphQlExpensesService.getFilteredExpenses(...this.filterParams).pipe(
                takeUntil(this.ngUnsubscribe),
                tap((result) => {
                    this.expenses = result?.data?.expenses_get_filtered_expenses as FilteredListResponseOfExpenseResponse;
                    this.loaderService.isBusy = false;
                }),
                handleApiError(this.snackBar),
                finalize(() => this.loaderService.isBusy = false)
            ).subscribe();
        }

        if (this.activeTab === 1) {
            this.graphQlExpensesService.getFilteredPlannedExpenses(...this.filterParams).pipe(
                takeUntil(this.ngUnsubscribe),
                tap((result) => {
                    this.plannedExpenses = result?.data?.expenses_get_filtered_planned_expenses as FilteredListResponseOfPlannedExpenseResponse;
                    this.loaderService.isBusy = false;
                }),
                handleApiError(this.snackBar),
                finalize(() => this.loaderService.isBusy = false)
            ).subscribe();
        }
    }

    public getCurrencyCode(balanceId : string | undefined): string {
        return this.currenciesMap?.get(Number(balanceId))?.code ?? 'USD';
    }

    public tabChanged(activeTab: number): void {
        this.activeTab = activeTab;
        if (this.activeTab === 0 && !this.expenses ||
            this.activeTab === 1 && !this.plannedExpenses) {
            this.getFilteredItems();
        }
    }
}
