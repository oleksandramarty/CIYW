import {Component, OnDestroy, OnInit} from '@angular/core';
import {finalize, of, Subject, switchMap, take, takeUntil, tap} from "rxjs";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {MatDialog} from "@angular/material/dialog";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {selectExpensesState} from "../../../../core/store/selectors/expenses.selectors";
import {Store} from "@ngrx/store";
import {DictionaryMap} from "../../../../core/models/common/dictionarie.model";
import {FormControl, FormGroup} from "@angular/forms";
import {DataItem} from "../../../../core/models/common/data-item.model";
import {CreateUpdateExpenseComponent} from "../../../dialogs/create-update-expense/create-update-expense.component";
import {ConfirmationMessageComponent} from "../../../dialogs/confirmation-message/confirmation-message.component";
import {getUTCString, handleBaseDateRangeFilter} from "../../../../core/helpers/date-time.helper";
import {LoaderService} from "../../../../core/services/loader.service";
import {GraphQlExpensesService} from "../../../../core/graph-ql/services/graph-ql-expenses.service";
import {CategoryResponse, CurrencyResponse} from "../../../../core/api-clients/dictionaries-client";
import {
    BaseSortableRequest, ColumnEnum, ExpenseResponse,
    ListWithIncludeResponseOfExpenseResponse, OrderDirectionEnum,
    PaginatorEntity, UserProjectResponse
} from "../../../../core/api-clients/common-module.client";
import {CommonDialogService} from "../../../../core/services/common-dialog.service";

@Component({
    selector: 'app-user-project',
    templateUrl: './user-project.component.html',
    styleUrl: './user-project.component.scss'
})
export class UserProjectComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    userProjectId: string | undefined;
    userProject: UserProjectResponse | undefined;
    expenses: ListWithIncludeResponseOfExpenseResponse | undefined;
    pagination: PaginatorEntity = new PaginatorEntity({pageSize: 10, pageNumber: 0, isFull: false});
    sort: BaseSortableRequest = new BaseSortableRequest({column: ColumnEnum.Date, direction: OrderDirectionEnum.Desc});

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
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
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
                descriptions: ['EXPENSES.DELETE_EXPENSE_CONFIRMATION']
            }
        });

        const removeExpenseAction = () => {
            this.graphQlExpensesService.removeExpense(expense.id!)
                .pipe(
                    takeUntil(this.ngUnsubscribe),
                    tap((result) => {
                        this.getExpenses();
                        this.getUserProjectFromApi();
                    }),
                    handleApiError(this.snackBar)
                )
                .subscribe();
        }

        this.commonDialogService.showNoComplaintModal(removeExpenseAction);
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
        this.loaderService.isBusy = true;
        this.graphQlExpensesService.getUserProjectById(this.userProjectId!)
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap(result => {
                    const userProject = result?.data?.expenses_get_user_project_by_id as UserProjectResponse;
                    this.userProject = userProject;
                    this.getExpenses();
                    this.loaderService.isBusy = false;
                }),
                handleApiError(this.snackBar),
                finalize(() => this.loaderService.isBusy = false)
            )
            .subscribe();
    }

    public resetFilter(): void {
        this.filterFormGroup.reset();
        this.getExpenses();
    }

    public getExpenses(): void {
        this.loaderService.isBusy = true;

        const dateRange = handleBaseDateRangeFilter(this.filterFormGroup!.value?.dateRange);

        this.graphQlExpensesService.getFilteredExpenses(
            dateRange?.startDate,
            dateRange?.endDate,
            undefined,
            undefined,
            this.pagination?.isFull ?? false,
            this.pagination?.pageNumber ?? 1,
            this.pagination?.pageSize ?? 10,
            this.sort?.column ?? ColumnEnum.Date,
            this.sort?.direction ?? OrderDirectionEnum.Desc,
            this.filterFormGroup?.value?.query ?? '',
            this.userProjectId!,
            this.filterFormGroup?.value.categoryIds?.split(',').map(Number) ?? []
        ).pipe(
            takeUntil(this.ngUnsubscribe),
            tap((result) => {
                const expenses = result?.data?.expenses_get_filtered_expenses as ListWithIncludeResponseOfExpenseResponse;
                this.expenses = expenses;
                this.loaderService.isBusy = false;
            }),
            handleApiError(this.snackBar),
            finalize(() => this.loaderService.isBusy = false)
        ).subscribe();
    }

    protected readonly Number = Number;
}
