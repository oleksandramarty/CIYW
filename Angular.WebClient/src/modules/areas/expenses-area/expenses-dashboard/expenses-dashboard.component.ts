import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject, switchMap, takeUntil, tap} from "rxjs";
import {
    ExpenseClient,
    UserAllowedProjectResponse,
    UserProjectResponse
} from "../../../../core/api-clients/expenses-client";
import {MatSnackBar} from "@angular/material/snack-bar";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";

@Component({
  selector: 'app-expenses-dashboard',
  templateUrl: './expenses-dashboard.component.html',
  styleUrl: './expenses-dashboard.component.scss'
})
export class ExpensesDashboardComponent implements OnInit, OnDestroy {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  userProjects: UserProjectResponse[] = [];
  allowedProjects: UserAllowedProjectResponse[] = [];

  constructor(
      private readonly expenseClient: ExpenseClient,
      private snackBar: MatSnackBar,
  ) {
  }

  public ngOnInit(): void {
    this.getUserProjects();
  }

  public ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  private getUserProjects(): void {
    this.expenseClient.userProject_GetProjects()
        .pipe(
            takeUntil(this.ngUnsubscribe),
            switchMap(userProjects => {
              this.userProjects = userProjects;
              return this.expenseClient.userProject_GetAllowedProjects();
            }),
            tap(allowedProjects => {
              this.allowedProjects = allowedProjects;
            }),
            handleApiError(this.snackBar)
            ).subscribe();
  }
}
