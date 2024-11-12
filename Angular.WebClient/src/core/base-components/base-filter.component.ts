import {Directive} from '@angular/core';
import {FormControl, FormGroup} from '@angular/forms';
import {MatSnackBar} from '@angular/material/snack-bar';
import {Observable, take, takeUntil, tap} from 'rxjs';
import {BaseUnsubscribeComponent} from './base-unsubscribe.compoinent';
import {
    BaseDateRangeFilterRequest,
    BaseSortableRequest, CategoryResponse,
    ColumnEnum,
    CurrencyResponse, FrequencyResponse, IconResponse,
    OrderDirectionEnum,
    PaginatorEntity
} from '../api-models/common.models';
import {getUTCString, handleBaseDateRangeFilter} from '../helpers/date-time.helper';
import {handleApiError} from '../helpers/rxjs.helper';
import {LoaderService} from '../services/loader.service';
import {LocalizationService} from '../services/localization.service';
import {DictionaryMap} from "../models/common/dictionary.model";
import {DataItem} from "../models/common/data-item.model";
import {DictionaryService} from "../services/dictionary.service";
import {BaseGraphQlFilteredModel} from "../models/common/base-graphql.model";

@Directive()
export abstract class BaseFilterComponent<TFilteredResponse, TFilterRequest> extends BaseUnsubscribeComponent {
    public columns = ColumnEnum;
    public directions = OrderDirectionEnum;

    public utcTimeShift: string = getUTCString();

    public filteredResult: TFilteredResponse | undefined;
    paginator: PaginatorEntity = new PaginatorEntity({pageSize: 10, pageNumber: 0, isFull: false});
    sort: BaseSortableRequest = new BaseSortableRequest({
        column: ColumnEnum.CreatedAt,
        direction: OrderDirectionEnum.Desc
    });

    public pageSizeOptions = [5, 10, 25];

    public filterFormGroup: FormGroup = new FormGroup({
        dateRange: new FormControl(),
        query: new FormControl()
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

    get frequenciesMap(): DictionaryMap<number, FrequencyResponse> | undefined {
        return this.dictionaryService.frequenciesMap;
    }

    get iconMap(): DictionaryMap<number, IconResponse> | undefined {
        return this.dictionaryService.iconMap;
    }

    protected constructor(
        protected readonly dictionaryService: DictionaryService,
        protected readonly localizationService: LocalizationService,
        protected readonly loaderService: LoaderService,
        protected readonly snackBar: MatSnackBar
    ) {
        super();
    }

    protected initializeFilterForm(snapshot$: Observable<any>): void {
        snapshot$
            .pipe(
                take(1),
                tap((result: any) => {
                    if (result) {
                        if (result.filteredResult) {
                            this.filteredResult = result.filteredResult;
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
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    override ngOnInit(): void {
        this.getFilteredItems();
    }

    public resetFilter(): void {
        this.filterFormGroup.reset();
        this.getFilteredItems();
    }

    get filterParams(): TFilterRequest {
        return this.createFilterParams();
    }

    protected get filterBaseModel(): BaseGraphQlFilteredModel {
        const dateRange = handleBaseDateRangeFilter(this.filterFormGroup!.value?.dateRange);

        return {
            dateFrom: dateRange?.startDate,
            dateTo: dateRange?.endDate,
            amountFrom: undefined,
            amountTo: undefined,
            isFull: this.paginator?.isFull ?? false,
            pageNumber: this.paginator?.pageNumber ?? 1,
            pageSize: this.paginator?.pageSize ?? 10,
            column: this.sort?.column?.toString() ?? ColumnEnum.Date.toString(),
            direction: this.sort?.direction?.toString() ?? OrderDirectionEnum.Desc.toString(),
            query: this.filterFormGroup?.value?.query ?? '',
        }
    }

    protected getCurrencyCode(balanceId: string | undefined): string {
        return this.currenciesMap?.get(Number(balanceId))?.code ?? 'USD';
    }

    protected abstract createFilterParams(): TFilterRequest;

    protected abstract getFilteredItemsSub(filterRequest: TFilterRequest): Observable<TFilteredResponse>;

    public getFilteredItems(): void {
        this.loaderService.isBusy = true;
        this.getFilteredItemsSub(this.filterParams)
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap(filteredItems => {
                    this.filteredResult = filteredItems;
                    this.loaderService.isBusy = false;
                }),
                handleApiError(this.snackBar, this.localizationService)
            ).subscribe();
    }

    public pageChanged(paginator: PaginatorEntity): void {
        this.paginator = paginator;
        this.getFilteredItems();
    }

    public sortItems(column: ColumnEnum): void {
        if (this.sort.column === column) {
            this.sort.direction =
                this.sort.direction === OrderDirectionEnum.Asc ?
                    OrderDirectionEnum.Desc :
                    OrderDirectionEnum.Asc;
        } else {
            this.sort.column = column;
        }
        this.getFilteredItems();
    }
}