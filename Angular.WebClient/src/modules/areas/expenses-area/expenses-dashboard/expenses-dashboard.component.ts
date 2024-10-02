import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject, switchMap, takeUntil, tap} from "rxjs";
import {
    ExpenseClient,
    UserAllowedProjectResponse,
    UserProjectResponse
} from "../../../../core/api-clients/expenses-client";
import {MatSnackBar} from "@angular/material/snack-bar";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";

interface Balance {
    currency: string;
    amount: number;
}

interface Account {
    name: string;
    balances: Balance[];
}

@Component({
  selector: 'app-expenses-dashboard',
  templateUrl: './expenses-dashboard.component.html',
  styleUrl: './expenses-dashboard.component.scss'
})
export class ExpensesDashboardComponent implements OnInit, OnDestroy {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  userProjects: UserProjectResponse[] = [];
  allowedProjects: UserAllowedProjectResponse[] = [];

    accounts: Account[] = [
        {
            name: 'Account 1',
            balances: [
                { currency: 'USD', amount: 1000 },
                { currency: 'EUR', amount: 800 },
                { currency: 'GBP', amount: 700 }
            ]
        },
        {
            name: 'Account 2',
            balances: [
                { currency: 'USD', amount: 500 },
                { currency: 'JPY', amount: 50000 }
            ]
        }
    ];

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
