import {Component} from '@angular/core';
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";
import {BaseFilterComponent} from "../../../../../core/base-components/base-filter.component";
import {
    AuditTrailActionEnum,
    AuditTrailEntityEnum, AuditTrailEnum, AuditTrailResponse, ExceptionEnum,
    FilteredListResponseOfAuditTrailResponse
} from "../../../../../core/api-models/common.models";
import {BaseGraphQlFilteredModel} from "../../../../../core/models/common/base-graphql.model";
import {
    createAdminAuditTrailHeader,
    ITableHeaderItem
} from "../../../../../core/models/table.model";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {LocalizationService} from "../../../../../core/services/localization.service";
import {LoaderService} from "../../../../../core/services/loader.service";
import {GraphQlExpensesService} from "../../../../../core/graph-ql/services/graph-ql-expenses.service";
import {CommonDialogService} from "../../../../../core/services/common-dialog.service";
import {FormControl} from "@angular/forms";
import {debounceTime, filter, Observable, take, takeUntil, tap} from "rxjs";
import {map} from "rxjs/operators";
import {
    createEnumDataItems,
    DataItem
} from "../../../../../core/models/common/data-item.model";

@Component({
    selector: 'app-admin-audit-trail',
    templateUrl: './admin-audit-trail.component.html',
    styleUrl: './admin-audit-trail.component.scss'
})
export class AdminAuditTrailComponent extends BaseFilterComponent<FilteredListResponseOfAuditTrailResponse, [
    BaseGraphQlFilteredModel,
        AuditTrailEntityEnum,
        AuditTrailActionEnum ,
        AuditTrailEnum,
        ExceptionEnum,
        string,
        string,
        string
]> {
    public tableHeaderItems: ITableHeaderItem[] = createAdminAuditTrailHeader();

    public auditTrailEntityDataItems: DataItem[] = createEnumDataItems(AuditTrailEntityEnum);
    public auditTrailActionTypeDataItems: DataItem[] = createEnumDataItems(ExceptionEnum);
    public auditTrailTypeDataItems: DataItem[] = createEnumDataItems(AuditTrailEnum);
    public exceptionTypeDataItems: DataItem[] = createEnumDataItems(AuditTrailActionEnum);

    public translationsDataItems: DataItem[] = [];

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
        this.filterFormGroup.addControl('entityType', new FormControl(undefined));
        this.filterFormGroup.addControl('action', new FormControl(undefined));
        this.filterFormGroup.addControl('type', new FormControl(undefined));
        this.filterFormGroup.addControl('exceptionType', new FormControl(undefined));
        this.filterFormGroup.addControl('entityId', new FormControl(undefined));
        this.filterFormGroup.addControl('userId', new FormControl(undefined));
        this.filterFormGroup.addControl('translationKey', new FormControl(undefined));

        this.localizationService.localeChangedSub
            .pipe(
                filter((state) => !!state),
                take(1),
                tap(() => {
                    setTimeout(() => {
                        this.translationsDataItems = this.localizationService.getAllLocalizationsDataItems;
                    }, 1000);
                })
            )
            .subscribe();

        super.ngOnInit();
    }

    protected createFilterParams(): [
        BaseGraphQlFilteredModel,
            AuditTrailEntityEnum,
            AuditTrailActionEnum,
            AuditTrailEnum,
            ExceptionEnum,
            string,
            string,
            string
    ] {
        return [
            this.filterBaseModel,
            this.filterFormGroup.get('entityType')?.value?.toString(),
            this.filterFormGroup.get('action')?.value?.toString(),
            this.filterFormGroup.get('type')?.value?.toString(),
            this.filterFormGroup.get('exceptionType')?.value?.toString(),
            this.filterFormGroup.get('entityId')?.value?.toString(),
            this.filterFormGroup.get('userId')?.value?.toString(),
            this.filterFormGroup.get('translationKey')?.value?.toString()
        ];
    }

    protected filteredItemsSub(filterRequest: [
        BaseGraphQlFilteredModel,
        AuditTrailEntityEnum,
        AuditTrailActionEnum,
        AuditTrailEnum,
        ExceptionEnum,
        string,
        string,
        string
    ]): Observable<FilteredListResponseOfAuditTrailResponse> {
        return this.graphQlExpensesService.filteredAuditTrail(...filterRequest).pipe(
            map(result => result.data.audit_trail_filtered_audit_trail!)
        );
    }

    public openDetails(entity: AuditTrailResponse): void {
        this.commonDialogService.showAuditTrailDetailsDialog(() => {}, () => {}, entity);
    }
}
