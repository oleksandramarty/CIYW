import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Observable, take, tap} from "rxjs";
import {
    BalanceResponse,
    FavoriteExpenseResponse,
    FilteredListResponseOfFavoriteExpenseResponse,
    UserProjectResponse
} from "../../../../../core/api-models/common.models";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {CommonDialogService} from "../../../../../core/services/common-dialog.service";
import {handleApiError} from "../../../../../core/helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LoaderService} from "../../../../../core/services/loader.service";
import {Store} from "@ngrx/store";
import {GraphQlExpensesService} from "../../../../../core/graph-ql/services/graph-ql-expenses.service";
import {selectExpensesSnapshot} from "../../../../../core/store/selectors/expenses.selectors";
import {expenses_setUserProject_favoriteExpensesSnapshot} from "../../../../../core/store/actions/expenses.actions";
import {BaseGraphQlFilteredModel} from "../../../../../core/models/common/base-graphql.model";
import {BaseFilterComponent} from "../../../../../core/base-components/base-filter.component";
import {LocalizationService} from "../../../../../core/services/localization.service";
import {FormControl} from "@angular/forms";
import {map} from "rxjs/operators";

@Component({
    selector: 'app-user-project-favorites',
    templateUrl: './user-project-favorites.component.html',
    styleUrl: '../user-project.component.scss',
    standalone: false
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
                        this.filteredItems();
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
            this.filteredItems();
        }, () => {}, favoriteExpense, this.userProject);
    }

    protected createFilterParams(): [BaseGraphQlFilteredModel, string, number[]] {
        return [
            this.filterBaseModel,
            this.userProject!.id,
            this.filterFormGroup?.value.categoryIds?.map(Number) ?? []
        ];
    }

    protected filteredItemsSub(filterRequest: [BaseGraphQlFilteredModel, string, number[]]): Observable<FilteredListResponseOfFavoriteExpenseResponse> {
        return this.graphQlExpensesService.filteredFavoriteExpenses(...filterRequest).pipe(
            map(result => result.data.expenses_filtered_favorite_expenses!)
        );
    }

    private _dragId: string | undefined;
    private _dropId: string | undefined;
    private _isBalanceDragged: boolean | undefined;

    get isDropForBalance(): boolean {
        return this._isBalanceDragged !== undefined && !this._isBalanceDragged;
    }

    get isDropForExpense(): boolean {
        return this._isBalanceDragged !== undefined && this._isBalanceDragged;
    }

    onDragStart(event: DragEvent, dragId: string, isBalanceDragged: boolean) {
        this._dragId = dragId;
        this._isBalanceDragged = isBalanceDragged;
        event.dataTransfer?.setData('text/plain', (event.target as HTMLElement).id);
    }

    onDragOver(event: DragEvent) {
        event.preventDefault();
    }

    onDrop(event: DragEvent, dropId: string) {
        event.preventDefault();
        this._dropId = dropId;
        this._triggerActionOnDrop();
    }

    private _triggerActionOnDrop() {
        const balanceIndex = this.userProject?.balances.findIndex(b => b.id === (this._isBalanceDragged ? this._dragId : this._dropId))
        const expenseIndex = this.filteredResult?.entities.findIndex(e => e.id === (this._isBalanceDragged ? this._dropId : this._dragId))
        this.commonDialogService.showCreateOrUpdateExpenseByFavoriteDialog(() => {
                this.favoritesChanged.emit();
                this.filteredItems();
                this._dragId = undefined;
                this._dropId = undefined;
                this._isBalanceDragged = undefined;
            }, () => {},
            balanceIndex !== -1 ? this.userProject?.balances[balanceIndex!] : undefined,
            expenseIndex !== -1 ? this.filteredResult?.entities[expenseIndex!] : undefined,
            this.userProject);
    }
}
