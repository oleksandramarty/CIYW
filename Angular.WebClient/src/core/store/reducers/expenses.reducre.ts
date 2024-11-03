import { createReducer, on } from '@ngrx/store';
import {
    expenses_clearAll,
    expenses_clearUserAllowedProjects,
    expenses_clearUserProject_expensesSnapshots, expenses_clearUserProject_favoriteExpensesSnapshots,
    expenses_clearUserProject_plannedExpensesSnapshots,
    expenses_clearUserProjects,
    expenses_setUserAllowedProjects,
    expenses_setUserProject,
    expenses_setUserProject_expensesSnapshot, expenses_setUserProject_favoriteExpensesSnapshot,
    expenses_setUserProject_plannedExpensesSnapshot,
    expenses_setUserProjects
} from "../actions/expenses.actions";
import {
    BaseSortableRequest,
    FilteredListResponseOfExpenseResponse,
    FilteredListResponseOfFavoriteExpenseResponse,
    FilteredListResponseOfPlannedExpenseResponse,
    FilteredListResponseOfUserAllowedProjectResponse,
    FilteredListResponseOfUserProjectResponse,
    PaginatorEntity
} from "../../api-models/common.models";

export interface ExpensesState {
    userProjects: FilteredListResponseOfUserProjectResponse | undefined;
    userAllowedProjects: FilteredListResponseOfUserAllowedProjectResponse | undefined;
    expensesSnapshot: {
        filteredResult: FilteredListResponseOfExpenseResponse | undefined,
        paginator: PaginatorEntity | undefined,
        sort: BaseSortableRequest | undefined,
        dateRange: any,
        query: any,
        categoryIds: any
    } | undefined,
    plannedExpensesSnapshot: {
        filteredResult: FilteredListResponseOfPlannedExpenseResponse | undefined,
        paginator: PaginatorEntity | undefined,
        sort: BaseSortableRequest | undefined,
        dateRange: any,
        query: any,
        categoryIds: any
    } | undefined,
    favoriteExpensesSnapshot: {
        filteredResult: FilteredListResponseOfFavoriteExpenseResponse | undefined,
        paginator: PaginatorEntity | undefined,
        sort: BaseSortableRequest | undefined,
        dateRange: any,
        query: any,
        categoryIds: any
    } | undefined
}

export const initialState: ExpensesState = {
    userProjects: undefined,
    userAllowedProjects: undefined,
    expensesSnapshot: undefined,
    plannedExpensesSnapshot: undefined,
    favoriteExpensesSnapshot: undefined
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
    on(expenses_clearAll, state => ({ ...state, userProjects: undefined, userAllowedProjects: undefined })),
    on(expenses_setUserProject_expensesSnapshot, (state, { filteredResult, paginator, sort, dateRange, query, categoryIds }) => ({
        ...state,
        expensesSnapshot: { filteredResult, paginator, sort, dateRange, query, categoryIds }
    })),
    on(expenses_setUserProject_plannedExpensesSnapshot, (state, { filteredResult, paginator, sort, dateRange, query, categoryIds }) => ({
        ...state,
        plannedExpensesSnapshot: { filteredResult, paginator, sort, dateRange, query, categoryIds }
    })),
    on(expenses_setUserProject_favoriteExpensesSnapshot, (state, { filteredResult, paginator, sort, dateRange, query, categoryIds }) => ({
        ...state,
        favoriteExpensesSnapshot: { filteredResult, paginator, sort, dateRange, query, categoryIds }
    })),
    on(expenses_clearUserProject_expensesSnapshots, state => ({
        ...state,
        expensesSnapshot: undefined,
    })),
    on(expenses_clearUserProject_plannedExpensesSnapshots, state => ({
        ...state,
        plannedExpensesSnapshot: undefined
    })),
    on(expenses_clearUserProject_favoriteExpensesSnapshots, state => ({
        ...state,
        favoriteExpensesSnapshot: undefined
    }))
);