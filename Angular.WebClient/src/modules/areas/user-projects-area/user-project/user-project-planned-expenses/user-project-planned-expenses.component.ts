import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {finalize, Subject, take, takeUntil, tap} from "rxjs";
import {
  BaseSortableRequest,
  CategoryResponse,
  ColumnEnum,
  CurrencyResponse,
  FilteredListResponseOfPlannedExpenseResponse,
  FrequencyResponse,
  OrderDirectionEnum,
  PaginatorEntity,
  PlannedExpenseResponse,
  UserProjectResponse
} from "../../../../../core/api-models/common.models";
import {getUTCString, handleBaseDateRangeFilter} from "../../../../../core/helpers/date-time.helper";
import {FormControl, FormGroup} from "@angular/forms";
import {DictionaryMap} from "../../../../../core/models/common/dictionary.model";
import {DataItem} from "../../../../../core/models/common/data-item.model";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Store} from "@ngrx/store";
import {LoaderService} from "../../../../../core/services/loader.service";
import {GraphQlExpensesService} from "../../../../../core/graph-ql/services/graph-ql-expenses.service";
import {CommonDialogService} from "../../../../../core/services/common-dialog.service";
import {handleApiError} from "../../../../../core/helpers/rxjs.helper";
import {BaseGraphQlFilteredModel} from "../../../../../core/models/common/base-graphql.model";
import {
  selectPlannedExpensesSnapshot
} from "../../../../../core/store/selectors/expenses.selectors";
import {expenses_setUserProject_plannedExpensesSnapshot} from "../../../../../core/store/actions/expenses.actions";

@Component({
  selector: 'app-user-project-planned-expenses',
  templateUrl: './user-project-planned-expenses.component.html',
  styleUrl: '../user-project.component.scss'
})
export class UserProjectPlannedExpensesComponent implements OnInit, OnDestroy {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  @Input() userProject: UserProjectResponse | undefined;
  @Output() plannedExpenseChanged: EventEmitter<void> = new EventEmitter();
  plannedExpenses: FilteredListResponseOfPlannedExpenseResponse | undefined;
  paginator: PaginatorEntity = new PaginatorEntity({pageSize: 10, pageNumber: 0, isFull: false});
  sort: BaseSortableRequest = new BaseSortableRequest({
    column: ColumnEnum.Date,
    direction: OrderDirectionEnum.Desc
  });

  public utcTimeShift: string = getUTCString();

  filterFormGroup: FormGroup = new FormGroup({
    dateRange: new FormControl(),
    query: new FormControl(),
    categoryIds: new FormControl(),
  });

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
      private readonly store: Store,
      private readonly loaderService: LoaderService,
      private readonly graphQlExpensesService: GraphQlExpensesService,
      private readonly commonDialogService: CommonDialogService
  ) {
  }

  public ngOnInit(): void {
    this.store.select(selectPlannedExpensesSnapshot)
        .pipe(
            take(1),
            tap((result) => {
              if (result) {
                if (result.plannedExpenses) {
                  this.plannedExpenses = result.plannedExpenses;
                }
                if (result.paginator) {
                  this.paginator = result.paginator;
                }
                if (result.sort) {
                  this.sort = result.sort;
                }
                if (result.dateRange) {
                  this.filterFormGroup.get('dateRange')?.setValue(result.dateRange);
                }
                if (result.query) {
                  this.filterFormGroup.get('query')?.setValue(result.query);
                }
                if (result.categoryIds) {
                  this.filterFormGroup.get('categoryIds')?.setValue(result.categoryIds);
                }
              } else {
                this.getFilteredItems();
              }
            }),
            handleApiError(this.snackBar)
        )
        .subscribe();
  }

  public ngOnDestroy(): void {
    this.store.dispatch(expenses_setUserProject_plannedExpensesSnapshot({
      plannedExpenses: this.plannedExpenses!,
      paginator: this.paginator,
      sort: this.sort,
      dateRange: this.filterFormGroup.value.dateRange,
      query: this.filterFormGroup.value.query,
      categoryIds: this.filterFormGroup.value.categoryIds
    }));
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  public openCreateUpdatePlannedExpenseDialog(plannedExpense: PlannedExpenseResponse | undefined): void {
    this.commonDialogService.showCreateOrUpdatePlannedExpenseModal(() => {
      this.getFilteredItems();
      this.plannedExpenseChanged.emit();
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
      this.commonDialogService.showRemoveExpenseConfirmationModal(removePlannedExpenseActionProceed);
    }
    this.commonDialogService.showNoComplaintModal(removePlannedExpenseAction);
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
        column: this.sort?.column?.toString() ?? ColumnEnum.NextDate.toString(),
        direction: this.sort?.direction?.toString() ?? OrderDirectionEnum.Desc.toString(),
        query: this.filterFormGroup?.value?.query ?? '',
      },
      this.userProject!.id,
      this.filterFormGroup?.value.categoryIds?.map(Number) ?? []
    ];
  }

  public getFilteredItems(): void {
    this.loaderService.isBusy = true;

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

  public getCurrencyCode(balanceId: string | undefined): string {
    return this.currenciesMap?.get(Number(balanceId))?.code ?? 'USD';
  }
}
