import {Component, OnDestroy, OnInit} from '@angular/core';
import {finalize, Subject, switchMap, take, takeUntil, tap} from "rxjs";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {
    ExpenseClient,
    UserAllowedProjectResponse,
    UserProjectResponse
} from "../../../../core/api-clients/expenses-client";
import {MatSnackBar} from "@angular/material/snack-bar";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {DictionaryMap} from "../../../../core/models/common/dictionarie.model";
import {MatDialog} from "@angular/material/dialog";
import {
    CreateUpdateUserProjectComponent
} from "../../../dialogs/create-update-user-project/create-update-user-project.component";
import {CurrencyResponse} from "../../../../core/api-clients/dictionaries-client";
import {Router} from "@angular/router";
import {Store} from "@ngrx/store";
import {selectExpensesState} from "../../../../core/store/selectors/expenses.selectors";
import {
    expenses_setUserAllowedProjects,
    expenses_setUserProjects
} from "../../../../core/store/actions/expenses.actions";

@Component({
    selector: 'app-user-projects',
    templateUrl: './user-projects.component.html',
    styleUrl: './user-projects.component.scss'
})
export class UserProjectsComponent implements OnInit, OnDestroy {
    protected ngUnsubscribe: Subject<void> = new Subject<void>();
    userProjects: UserProjectResponse[] | undefined;
    userAllowedProjects: UserAllowedProjectResponse[] | undefined;
    isBusy: boolean = false;

    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        return this.dictionaryService.currenciesMap;
    }

    constructor(
        private readonly dictionaryService: DictionaryService,
        private readonly expenseClient: ExpenseClient,
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private router: Router,
        private readonly store: Store
    ) {
    }

    public ngOnInit(): void {
        this.store.select(selectExpensesState)
            .pipe(
                take(1),
                tap(expensesState => {
                    if (!!expensesState.userProjects && !!expensesState.userAllowedProjects) {
                        this.userProjects = expensesState.userProjects;
                        this.userAllowedProjects = expensesState.userAllowedProjects;
                    } else {
                        this.getUserProjects();
                    }
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    public ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public openCreateUserProjectDialog(): void {
        const dialogRef = this.dialog.open(CreateUpdateUserProjectComponent, {
            width: '400px',
            maxWidth: '80vw',
            data: {}
        });

        dialogRef.afterClosed().subscribe(result => {
            if (!!result) {
                this.getUserProjects();
            }
        });
    }

    public openUserProject(id: string | undefined): void {
        if (!id) {
            return;
        }

        this.router.navigate(['/projects', id]);
    }

    private getUserProjects(): void {
        this.isBusy = true;
        this.expenseClient.userProject_GetProjects()
            .pipe(
                takeUntil(this.ngUnsubscribe),
                switchMap(userProjects => {
                    this.userProjects = userProjects;
                    this.store.dispatch(expenses_setUserProjects({ userProjects }));
                    return this.expenseClient.userProject_GetAllowedProjects();
                }),
                tap(userAllowedProjects => {
                    this.userAllowedProjects = userAllowedProjects;
                    this.store.dispatch(expenses_setUserAllowedProjects({ userAllowedProjects }));
                }),
                handleApiError(this.snackBar),
                finalize(() => this.isBusy = false)
            ).subscribe();
    }
}
