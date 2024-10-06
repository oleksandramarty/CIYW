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
export class ExpensesDashboardComponent {
}
