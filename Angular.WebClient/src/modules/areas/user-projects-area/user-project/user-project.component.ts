import {Component, OnDestroy, OnInit} from '@angular/core';
import {finalize, Subject, take, takeUntil, tap} from "rxjs";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {selectExpensesState} from "../../../../core/store/selectors/expenses.selectors";
import {Store} from "@ngrx/store";
import {DictionaryMap} from "../../../../core/models/common/dictionary.model";
import {LoaderService} from "../../../../core/services/loader.service";
import {GraphQlExpensesService} from "../../../../core/graph-ql/services/graph-ql-expenses.service";
import {CommonDialogService} from "../../../../core/services/common-dialog.service";
import {fadeInOut} from "../../../../core/animations/animations";
import {
    BalanceTypeResponse,
    CurrencyResponse,
    UserProjectResponse
} from "../../../../core/api-models/common.models";

@Component({
    selector: 'app-user-project',
    templateUrl: './user-project.component.html',
    styleUrl: './user-project.component.scss',
    animations: [fadeInOut]
})
export class UserProjectComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    public userProjectId: string | undefined;
    public userProject: UserProjectResponse | undefined;
    public activeTab: number = 0;

    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        return this.dictionaryService.currenciesMap;
    }

    get balanceTypesMap(): DictionaryMap<number, BalanceTypeResponse> | undefined {
        return this.dictionaryService.balanceTypesMap;
    }

    constructor(
        private readonly dictionaryService: DictionaryService,
        private readonly snackBar: MatSnackBar,
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

    public getUserProjectFromApi(): void {
        this.loaderService.isBusy = true;
        this.graphQlExpensesService.getUserProjectById(this.userProjectId!)
            .pipe(
                takeUntil(this.ngUnsubscribe),
                tap(result => {
                    const userProject = result?.data?.expenses_get_user_project_by_id as UserProjectResponse;
                    this.userProject = userProject;
                    this.loaderService.isBusy = false;
                }),
                handleApiError(this.snackBar),
                finalize(() => this.loaderService.isBusy = false)
            )
            .subscribe();
    }
}
