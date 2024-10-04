import {Component, OnDestroy, OnInit} from '@angular/core';
import {finalize, of, Subject, switchMap, take, takeUntil, tap} from "rxjs";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {
    BaseDateRangeFilterRequest,
    BaseSortableRequest,
    ExpenseClient, ExpenseResponse,
    GetFilteredExpensesRequest, ListWithIncludeResponseOfExpenseResponse, PaginatorEntity,
    UserProjectResponse
} from "../../../../core/api-clients/expenses-client";
import {MatSnackBar} from "@angular/material/snack-bar";
import {MatDialog} from "@angular/material/dialog";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {selectExpensesState} from "../../../../core/store/selectors/expenses.selectors";
import {Store} from "@ngrx/store";
import {CategoryResponse, CurrencyResponse} from "../../../../core/api-clients/dictionaries-client";
import {DictionaryMap} from "../../../../core/models/common/dictionarie.model";
import {FormControl, FormGroup} from "@angular/forms";
import {DataItem} from "../../../../core/models/common/data-item.model";
import {CreateUpdateExpenseComponent} from "../../../dialogs/create-update-expense/create-update-expense.component";
import {ConfirmationMessageComponent} from "../../../dialogs/confirmation-message/confirmation-message.component";
import {getUTCString, handleBaseDateRangeFilter} from "../../../../core/helpers/date-time.helper";

@Component({
    selector: 'app-user-project',
    templateUrl: './user-project.component.html',
    styleUrl: './user-project.component.scss'
})
export class UserProjectComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    isBusy: boolean = false;
    userProjectId: string | undefined;
    userProject: UserProjectResponse | undefined;
    expenses: ListWithIncludeResponseOfExpenseResponse | undefined;
    pagination: PaginatorEntity = new PaginatorEntity({pageSize: 10, pageNumber: 0, isFull: false});
    sort: BaseSortableRequest = new BaseSortableRequest({column: 'Date', direction: 'desc'});

    public utcTimeShift: string = getUTCString();

    filterFormGroup: FormGroup = new FormGroup({
        dateRange: new FormControl(),
        query: new FormControl(),
        categoryIds: new FormControl(),
    });

    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        return this.dictionaryService.currenciesMap;
    }

    get categoriesMap(): DictionaryMap<number, CategoryResponse> | undefined {
        return this.dictionaryService.categoriesMap;
    }

    get categoriesDataItems(): DataItem[] {
        return this.dictionaryService.dataItems?.categories ?? [];
    }

    constructor(
        private readonly dictionaryService: DictionaryService,
        private readonly expenseClient: ExpenseClient,
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private route: ActivatedRoute,
        private router: Router,
        private readonly store: Store
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
        const dialogRef = this.dialog.open(CreateUpdateExpenseComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                expense,
                userProject: this.userProject
            }
        });

        dialogRef.afterClosed()
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap((result) => {
                    if (!!result) {
                        this.getExpenses();
                        this.getUserProjectFromApi();
                    }
                })
            )
            .subscribe();
    }

    public removeExpense(expense: ExpenseResponse): void {
        const dialogRef = this.dialog.open(ConfirmationMessageComponent, {
            width: '400px',
            maxWidth: '80vw',
            data: {
                yesBtn: 'COMMON.YES',
                noBtn: 'COMMON.NO',
                title: 'EXPENSES.DELETE_EXPENSE',
                description: 'EXPENSES.DELETE_EXPENSE_CONFIRMATION'
            }
        });

        dialogRef.afterClosed()
            .pipe(
                takeUntil(this.ngUnsubscribe),
                switchMap((result) => {
                    return result ? this.expenseClient.expense_RemoveExpense(expense.id!) : of(false);
                }),
                tap((result) => {
                    if (result) {
                        this.getExpenses();
                        this.getUserProjectFromApi();
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    private getUserProject(): void {
        this.store.select(selectExpensesState)
            .pipe(
                take(1),
                tap(expensesState => {
                    if (!!expensesState.userProjects && !!expensesState.userAllowedProjects) {
                        const userProject = expensesState.userProjects?.find(project => project.id === this.userProjectId);
                        const userAllowedProject = expensesState.userProjects?.find(project => project.id === this.userProjectId);
                        if (!!userProject || !!userAllowedProject) {
                            this.userProject = userProject ?? userAllowedProject;
                            this.getExpenses();
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
        this.isBusy = true;
        this.expenseClient.userProject_GetUserProject(this.userProjectId!)
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap(userProject => {
                    this.userProject = userProject;
                    this.getExpenses();
                    this.isBusy = false;
                }),
                handleApiError(this.snackBar),
                finalize(() => this.isBusy = false)
            )
            .subscribe();
    }

    public resetFilter(): void {
        this.filterFormGroup.reset();
        this.getExpenses();
    }

    public getExpenses(): void {
        this.isBusy = true;

        this.expenseClient.expense_GetFilteredExpenses(new GetFilteredExpensesRequest({
            userProjectId: this.userProjectId!,
            paginator: this.pagination,
            sort: this.sort,
            dateRange: handleBaseDateRangeFilter(this.filterFormGroup!.value?.dateRange),
            query: this.filterFormGroup!.value?.query ?? '',
            categoryIds: this.filterFormGroup!.value.categoryIds?.split(',').map(Number) ?? [],
        })).pipe(
            takeUntil(this.ngUnsubscribe),
            tap(expenses => {
                this.expenses = expenses;
                this.isBusy = false;
            }),
            handleApiError(this.snackBar),
            finalize(() => this.isBusy = false)
        ).subscribe();
    }
}
