import { ActionReducerMap } from '@ngrx/store';
import {authReducer, AuthState} from "./auth.reducer";
import {expensesReducer, ExpensesState} from "./expenses.reducre";

export interface AppState {
  auth: AuthState;
  expenses: ExpensesState;
}

export const reducers: ActionReducerMap<AppState> = {
  auth: authReducer,
  expenses: expensesReducer
};
