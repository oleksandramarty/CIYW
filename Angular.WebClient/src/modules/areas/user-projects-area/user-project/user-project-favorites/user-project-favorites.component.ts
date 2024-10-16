import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {finalize, Subject, take, takeUntil, tap} from "rxjs";
import {
  BalanceResponse,
  BaseSortableRequest,
  ColumnEnum,
  CurrencyResponse,
  FavoriteExpenseResponse,
  FilteredListResponseOfFavoriteExpenseResponse,
  FilteredListResponseOfPlannedExpenseResponse,
  IconResponse,
  OrderDirectionEnum,
  PaginatorEntity,
  UserProjectResponse
} from "../../../../../core/api-models/common.models";
import {DictionaryMap} from "../../../../../core/models/common/dictionary.model";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {CommonDialogService} from "../../../../../core/services/common-dialog.service";
import {handleApiError} from "../../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LoaderService} from "../../../../../core/services/loader.service";
import {Store} from "@ngrx/store";
import {GraphQlExpensesService} from "../../../../../core/graph-ql/services/graph-ql-expenses.service";
import {
  selectExpensesSnapshot,
  selectFavoriteExpensesSnapshot
} from "../../../../../core/store/selectors/expenses.selectors";
import {
  expenses_setUserProject_expensesSnapshot,
  expenses_setUserProject_favoriteExpensesSnapshot
} from "../../../../../core/store/actions/expenses.actions";
import {BaseGraphQlFilteredModel} from "../../../../../core/models/common/base-graphql.model";
import {handleBaseDateRangeFilter} from "../../../../../core/helpers/date-time.helper";

@Component({
    selector: 'app-user-project-favorites',
    templateUrl: './user-project-favorites.component.html',
    styleUrl: '../user-project.component.scss'
})
export class UserProjectFavoritesComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    @Input() userProject: UserProjectResponse | undefined;
    @Output() favoritesChanged: EventEmitter<void> = new EventEmitter();
    favoriteExpenses: FilteredListResponseOfFavoriteExpenseResponse | undefined;
    paginator: PaginatorEntity = new PaginatorEntity({pageSize: 10, pageNumber: 0, isFull: false});
    sort: BaseSortableRequest = new BaseSortableRequest({
        column: ColumnEnum.Created,
        direction: OrderDirectionEnum.Desc
    });

    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        return this.dictionaryService.currenciesMap;
    }

    get iconMap(): DictionaryMap<number, IconResponse> | undefined {
        return this.dictionaryService.iconMap;
    }

    constructor(
        private readonly dictionaryService: DictionaryService,
        private readonly commonDialogService: CommonDialogService,
        private readonly snackBar: MatSnackBar,
        private readonly loaderService: LoaderService,
        private readonly store: Store,
        private readonly graphQlExpensesService: GraphQlExpensesService
    ) {
    }

    public ngOnInit(): void {
      this.store.select(selectFavoriteExpensesSnapshot)
          .pipe(
              take(1),
              tap((result) => {
                if (result) {
                  if (result.favoriteExpenses) {
                    this.favoriteExpenses = result.favoriteExpenses;
                  }
                  if (result.paginator) {
                    this.paginator = result.paginator;
                  }
                  if (result.sort) {
                    this.sort = result.sort;
                  }
                  // if (result.dateRange) {
                  //   this.filterFormGroup.get('dateRange')?.setValue(result.dateRange);
                  // }
                  // if (result.query) {
                  //   this.filterFormGroup.get('query')?.setValue(result.query);
                  // }
                  // if (result.categoryIds) {
                  //   this.filterFormGroup.get('categoryIds')?.setValue(result.categoryIds);
                  // }
                } else {
                  this.getFilteredItems();
                }
              }),
              handleApiError(this.snackBar)
          )
          .subscribe();
    }

    public ngOnDestroy(): void {
      this.store.dispatch(expenses_setUserProject_favoriteExpensesSnapshot({
        favoriteExpenses: this.favoriteExpenses!,
        paginator: this.paginator,
        sort: this.sort,
        dateRange: undefined, // this.filterFormGroup.value.dateRange,
        query: undefined, // this.filterFormGroup.value.query,
        categoryIds: undefined, // this.filterFormGroup.value.categoryIds
      }));
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public openBalanceDialog(balance: BalanceResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdateUserBalanceModal(() => {
            this.favoritesChanged.emit();
        }, balance, this.userProject);
    }

    public openFavoriteExpenseDialog(favoriteExpense: FavoriteExpenseResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdateFavoriteExpenseModal(() => {
            this.getFilteredItems();
        }, favoriteExpense, this.userProject);
    }

  get filterParams(): [BaseGraphQlFilteredModel, string, number[]] {
    // const dateRange = handleBaseDateRangeFilter(this.filterFormGroup!.value?.dateRange);

    return [
      {
        dateFrom: undefined, //dateRange?.startDate,
        dateTo: undefined, //dateRange?.endDate,
        amountFrom: undefined,
        amountTo: undefined,
        isFull: this.paginator?.isFull ?? false,
        pageNumber: this.paginator?.pageNumber ?? 1,
        pageSize: this.paginator?.pageSize ?? 10,
        column: this.sort?.column?.toString() ?? ColumnEnum.Date.toString(),
        direction: this.sort?.direction?.toString() ?? OrderDirectionEnum.Desc.toString(),
        query: '' // this.filterFormGroup?.value?.query ?? '',
      },
      this.userProject!.id,
      [] //this.filterFormGroup?.value.categoryIds?.map(Number) ?? []
    ];
  }

    public getFilteredItems(): void {
        this.loaderService.isBusy = true;

        this.graphQlExpensesService.getFilteredFavoriteExpenses(...this.filterParams).pipe(
            takeUntil(this.ngUnsubscribe),
            tap((result) => {
                this.favoriteExpenses = result?.data?.expenses_get_filtered_favorite_expenses as FilteredListResponseOfFavoriteExpenseResponse;
                this.loaderService.isBusy = false;
            }),
            handleApiError(this.snackBar),
            finalize(() => this.loaderService.isBusy = false)
        ).subscribe();
    }

    private _dragIndex: number = -1;
    private _dropIndex: number = -1;

    onDragStart(event: DragEvent, index: number) {
        this._dragIndex = index;
        event.dataTransfer?.setData('text/plain', (event.target as HTMLElement).id);
    }

    onDragOver(event: DragEvent) {
        event.preventDefault();
    }

    onDrop(event: DragEvent, index: number) {
        event.preventDefault();
        this._dropIndex = index;
        this._triggerActionOnDrop();
    }

    private _triggerActionOnDrop() {
        this.commonDialogService.showCreateOrUpdateExpenseByFavoriteModal(() => {
            this._dragIndex = -1;
            this._dropIndex = -1;
        },
            this.userProject?.balances[this._dragIndex],
            this.favoriteExpenses?.entities[this._dropIndex],
            this.userProject);
    }
}
