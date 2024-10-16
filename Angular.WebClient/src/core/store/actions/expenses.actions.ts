import {createAction, props} from "@ngrx/store";
import {
    BaseSortableRequest,
    ColumnEnum,
    FilteredListResponseOfExpenseResponse,
    FilteredListResponseOfFavoriteExpenseResponse,
    FilteredListResponseOfPlannedExpenseResponse,
    FilteredListResponseOfUserAllowedProjectResponse,
    FilteredListResponseOfUserProjectResponse,
    OrderDirectionEnum,
    PaginatorEntity,
    UserProjectResponse
} from "../../api-models/common.models";
import {BaseGraphQlFilteredModel} from "../../models/common/base-graphql.model";
import {FormControl, FormGroup} from "@angular/forms";

export const expenses_setUserProject = createAction('[Expenses] Set UserProject', props<{
    userProject: UserProjectResponse
}>());
export const expenses_setUserProjects = createAction('[Expenses] Set UserProjects', props<{
    userProjects: FilteredListResponseOfUserProjectResponse
}>());
export const expenses_clearUserProjects = createAction('[Expenses] Clear UserProjects');
export const expenses_setUserAllowedProjects = createAction('[Expenses] Set UserAllowedProjects', props<{
    userAllowedProjects: FilteredListResponseOfUserAllowedProjectResponse
}>());
export const expenses_clearUserAllowedProjects = createAction('[Expenses] Clear UserAllowedProjects');
export const expenses_clearAll = createAction('[Expenses] Clear All');
export const expenses_setUserProject_expensesSnapshot = createAction(
    '[Expenses] Set UserProject Expenses Snapshot',
    props<{
        expenses: FilteredListResponseOfExpenseResponse | undefined;
        paginator: PaginatorEntity | undefined;
        sort: BaseSortableRequest | undefined;
        dateRange: any;
        query: any;
        categoryIds: any;
    }>()
);
export const expenses_setUserProject_plannedExpensesSnapshot = createAction(
    '[Expenses] Set UserProject Planned Expenses Snapshot',
    props<{
        plannedExpenses: FilteredListResponseOfPlannedExpenseResponse | undefined;
        paginator: PaginatorEntity | undefined;
        sort: BaseSortableRequest | undefined;
        dateRange: any;
        query: any;
        categoryIds: any;
    }>()
);
export const expenses_setUserProject_favoriteExpensesSnapshot = createAction(
    '[Expenses] Set UserProject Favorite Expenses Snapshot',
    props<{
        favoriteExpenses: FilteredListResponseOfFavoriteExpenseResponse | undefined;
        paginator: PaginatorEntity | undefined;
        sort: BaseSortableRequest | undefined;
        dateRange: any;
        query: any;
        categoryIds: any;
    }>()
);
export const expenses_clearUserProject_expensesSnapshots = createAction('[Expenses] Clear UserProject Expenses Snapshots');
export const expenses_clearUserProject_plannedExpensesSnapshots = createAction('[Expenses] Clear UserProject Planned Expenses Snapshots');
export const expenses_clearUserProject_favoriteExpensesSnapshots = createAction('[Expenses] Clear UserProject Favorite Expenses Snapshots');