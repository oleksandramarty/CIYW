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
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrl: './dashboard.component.scss',
    standalone: false
})
export class DashboardComponent {
}
