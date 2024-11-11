import {ActionReducerMap} from '@ngrx/store';
import {authReducer, AuthState} from "./auth.reducer";
import {expensesReducer, ExpensesState} from "./expenses.reducre";
import {siteReducer, SiteState} from "./site.reducre";

export interface AppState {
    auth: AuthState;
    expenses: ExpensesState;
    site: SiteState;
}

export const reducers: ActionReducerMap<AppState> = {
    auth: authReducer,
    expenses: expensesReducer,
    site: siteReducer
};
