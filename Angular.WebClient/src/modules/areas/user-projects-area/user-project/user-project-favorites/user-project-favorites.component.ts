import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {finalize, Observable, Subject, take, takeUntil, tap} from "rxjs";
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
import {BaseUnsubscribeComponent} from "../../../../../core/base-components/base-unsubscribe.compoinent";
import {BaseFilterComponent} from "../../../../../core/base-components/base-filter.component";
import {LocalizationService} from "../../../../../core/services/localization.service";
import {FormControl} from "@angular/forms";
import {map} from "rxjs/operators";

@Component({
    selector: 'app-user-project-favorites',
    templateUrl: './user-project-favorites.component.html',
    styleUrl: '../user-project.component.scss'
})
export class UserProjectFavoritesComponent extends BaseFilterComponent<FilteredListResponseOfFavoriteExpenseResponse, [BaseGraphQlFilteredModel, string, number[]]> {
    @Input() userProject: UserProjectResponse | undefined;
    @Output() favoritesChanged: EventEmitter<void> = new EventEmitter();

    constructor(
        protected override readonly dictionaryService: DictionaryService,
        protected override readonly localizationService: LocalizationService,
        protected override readonly loaderService: LoaderService,
        protected override readonly snackBar: MatSnackBar,
        private readonly commonDialogService: CommonDialogService,
        private readonly store: Store,
        private readonly graphQlExpensesService: GraphQlExpensesService
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
                        this.getFilteredItems();
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    override ngOnDestroy(): void {
        this.store.dispatch(expenses_setUserProject_favoriteExpensesSnapshot({
            filteredResult: this.filteredResult!,
            paginator: this.paginator,
            sort: this.sort,
            dateRange: undefined, // this.filterFormGroup.value.dateRange,
            query: undefined, // this.filterFormGroup.value.query,
            categoryIds: undefined, // this.filterFormGroup.value.categoryIds
        }));
        super.ngOnDestroy();
    }

    public openBalanceDialog(balance: BalanceResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdateUserBalanceDialog(() => {
            this.favoritesChanged.emit();
        }, () => {}, balance, this.userProject);
    }

    public openFavoriteExpenseDialog(favoriteExpense: FavoriteExpenseResponse | undefined): void {
        this.commonDialogService.showCreateOrUpdateFavoriteExpenseDialog(() => {
            this.getFilteredItems();
        }, () => {}, favoriteExpense, this.userProject);
    }

    protected createFilterParams(): [BaseGraphQlFilteredModel, string, number[]] {
        return [
            this.filterBaseModel,
            this.userProject!.id,
            this.filterFormGroup?.value.categoryIds?.map(Number) ?? []
        ];
    }

    protected getFilteredItemsSub(filterRequest: [BaseGraphQlFilteredModel, string, number[]]): Observable<FilteredListResponseOfFavoriteExpenseResponse> {
        return this.graphQlExpensesService.getFilteredFavoriteExpenses(...filterRequest).pipe(
            map(result => result.data.expenses_get_filtered_favorite_expenses!)
        );
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
        this.commonDialogService.showCreateOrUpdateExpenseByFavoriteDialog(() => {
                this.favoritesChanged.emit();
                this.getFilteredItems();
                this._dragIndex = -1;
                this._dropIndex = -1;
            }, () => {},
            this.userProject?.balances[this._dragIndex],
            this.filteredResult?.entities[this._dropIndex],
            this.userProject);
    }
}
