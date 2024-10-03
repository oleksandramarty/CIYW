import { createReducer, on } from '@ngrx/store';
import {UserAllowedProjectResponse, UserProjectResponse} from "../../api-clients/expenses-client";
import {
    expenses_clearAll,
    expenses_clearUserAllowedProjects, expenses_clearUserProjects,
    expenses_setUserAllowedProjects, expenses_setUserProject,
    expenses_setUserProjects
} from "../actions/expenses.actions";

export interface ExpensesState {
    userProjects: UserProjectResponse[] | undefined;
    userAllowedProjects: UserAllowedProjectResponse[] | undefined;
}

export const initialState: ExpensesState = {
    userProjects: undefined,
    userAllowedProjects: undefined,
};

export const expensesReducer = createReducer(
    initialState,
    on(expenses_setUserProject, (state, { userProject }) => ({ ...state, userProjects: [...(state.userProjects || []), userProject] })),
    on(expenses_setUserProjects, (state, { userProjects }) => ({ ...state, userProjects })),
    on(expenses_clearUserProjects, state => ({ ...state, userProjects: undefined })),
    on(expenses_setUserAllowedProjects, (state, { userAllowedProjects }) => ({ ...state, userAllowedProjects })),
    on(expenses_clearUserAllowedProjects, state => ({ ...state, userAllowedProjects: undefined })),
    on(expenses_clearAll, state => ({ ...state, userProjects: undefined, userAllowedProjects: undefined }))
);
