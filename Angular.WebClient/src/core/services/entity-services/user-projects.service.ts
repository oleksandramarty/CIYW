import {Injectable} from "@angular/core";
import {ExpenseClient, UserAllowedProjectResponse, UserProjectResponse} from "../../api-clients/expenses-client";
import {DictionaryService} from "../dictionary.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {MatDialog} from "@angular/material/dialog";
import {Router} from "@angular/router";
import {Store} from "@ngrx/store";
import {LoaderService} from "../loader.service";
import {selectExpensesState} from "../../store/selectors/expenses.selectors";
import {finalize, Subject, switchMap, take, takeUntil, tap} from "rxjs";
import {handleApiError} from "../../helpers/rxjs.helper";
import {
    CreateUpdateUserProjectComponent
} from "../../../modules/dialogs/create-update-user-project/create-update-user-project.component";
import {expenses_setUserAllowedProjects, expenses_setUserProjects} from "../../store/actions/expenses.actions";

@Injectable({
    providedIn: "root"
})
export class UserProjectsService {
    private _userProjects: UserProjectResponse[] | undefined;
    private _userAllowedProjects: UserAllowedProjectResponse[] | undefined;

    constructor(
        private readonly expenseClient: ExpenseClient,
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private router: Router,
        private readonly store: Store,
        private readonly loaderService: LoaderService
    ) {
    }

    get userProjects(): UserProjectResponse[] | undefined {
        return this._userProjects;
    }

    get userAllowedProjects(): UserAllowedProjectResponse[] | undefined {
        return this._userAllowedProjects;
    }

    public initProjects(ngUnsubscribe: Subject<void>): void {
        this.store.select(selectExpensesState)
            .pipe(
                take(1),
                tap(expensesState => {
                    if (!!expensesState.userProjects && !!expensesState.userAllowedProjects) {
                        this._userProjects = expensesState.userProjects;
                        this._userAllowedProjects = expensesState.userAllowedProjects;
                    } else {
                        this.getUserProjects(ngUnsubscribe);
                    }
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    public openCreateUserProjectDialog(ngUnsubscribe: Subject<void>): void {
        const dialogRef = this.dialog.open(CreateUpdateUserProjectComponent, {
            width: '400px',
            maxWidth: '80vw',
            data: {}
        });

        dialogRef.afterClosed()
            .pipe(
                takeUntil(ngUnsubscribe),
                tap((result) => {
                    if (!!result) {
                        this.getUserProjects(ngUnsubscribe);
                    }
                })
            )
            .subscribe();
    }

    public openUserProject(id: string | undefined): void {
        if (!id) {
            return;
        }

        this.router.navigate(['/projects', id]);
    }

    private getUserProjects(ngUnsubscribe: Subject<void>): void {
        this.loaderService.isBusy = true;
        this.expenseClient.userProject_GetProjects()
            .pipe(
                takeUntil(ngUnsubscribe),
                switchMap(userProjects => {
                    this._userProjects = userProjects;
                    this.store.dispatch(expenses_setUserProjects({ userProjects }));
                    return this.expenseClient.userProject_GetAllowedProjects();
                }),
                tap(userAllowedProjects => {
                    this._userAllowedProjects = userAllowedProjects;
                    this.store.dispatch(expenses_setUserAllowedProjects({ userAllowedProjects }));
                }),
                handleApiError(this.snackBar),
                finalize(() => this.loaderService.isBusy = false)
            ).subscribe();
    }
}