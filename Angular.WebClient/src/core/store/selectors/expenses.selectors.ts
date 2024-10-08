import { createSelector, createFeatureSelector } from '@ngrx/store';
import {ExpensesState} from "../reducers/expenses.reducre";

export const selectExpensesState = createFeatureSelector<ExpensesState>('expenses');

export const selectUserProjects = createSelector(
    selectExpensesState,
    (state: ExpensesState) => state.userProjects
);

export const selectUserAllowedProjects = createSelector(
    selectExpensesState,
    (state: ExpensesState) => state.userAllowedProjects
);
