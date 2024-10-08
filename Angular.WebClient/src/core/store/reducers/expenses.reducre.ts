import { createReducer, on } from '@ngrx/store';
import {
    expenses_clearAll,
    expenses_clearUserAllowedProjects, expenses_clearUserProjects,
    expenses_setUserAllowedProjects, expenses_setUserProject,
    expenses_setUserProjects
} from "../actions/expenses.actions";
import {
    FilteredListResponseOfUserAllowedProjectResponse,
    FilteredListResponseOfUserProjectResponse,
    UserAllowedProjectResponse,
    UserProjectResponse
} from "../../api-clients/common-module.client";

export interface ExpensesState {
    userProjects: FilteredListResponseOfUserProjectResponse | undefined;
    userAllowedProjects: FilteredListResponseOfUserAllowedProjectResponse | undefined;
}

export const initialState: ExpensesState = {
    userProjects: undefined,
    userAllowedProjects: undefined,
};

export const expensesReducer = createReducer(
    initialState,
on(expenses_setUserProject, (state, { userProject }) => {
    const existingProjectIndex = state.userProjects?.entities?.findIndex(project => project.id === userProject.id);
    const updatedEntities = existingProjectIndex !== undefined && existingProjectIndex >= 0
        ? state.userProjects?.entities.map((project, index) => index === existingProjectIndex ? userProject : project)
        : [...(state.userProjects?.entities || []), userProject];

    return {
        ...state,
        userProjects: {
            ...state.userProjects,
            entities: updatedEntities
        } as FilteredListResponseOfUserProjectResponse
    };
}),
    on(expenses_setUserProjects, (state, { userProjects }) => ({ ...state, userProjects })),
    on(expenses_clearUserProjects, state => ({ ...state, userProjects: undefined })),
    on(expenses_setUserAllowedProjects, (state, { userAllowedProjects }) => ({ ...state, userAllowedProjects })),
    on(expenses_clearUserAllowedProjects, state => ({ ...state, userAllowedProjects: undefined })),
    on(expenses_clearAll, state => ({ ...state, userProjects: undefined, userAllowedProjects: undefined }))
);
