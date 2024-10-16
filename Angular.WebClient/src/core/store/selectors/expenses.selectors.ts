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

export const selectExpensesSnapshot = createSelector(
    selectExpensesState,
    (state: ExpensesState) => state.expensesSnapshot
);

export const selectPlannedExpensesSnapshot = createSelector(
    selectExpensesState,
    (state: ExpensesState) => state.plannedExpensesSnapshot
);

export const selectFavoriteExpensesSnapshot = createSelector(
    selectExpensesState,
    (state: ExpensesState) => state.favoriteExpensesSnapshot
);