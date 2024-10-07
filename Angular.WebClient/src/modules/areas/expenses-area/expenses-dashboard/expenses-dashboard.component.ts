import {Component} from '@angular/core';

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
