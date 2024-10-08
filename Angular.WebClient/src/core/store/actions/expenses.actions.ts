import {createAction, props} from "@ngrx/store";
import {
    FilteredListResponseOfUserAllowedProjectResponse,
    FilteredListResponseOfUserProjectResponse,
    UserProjectResponse
} from "../../api-clients/common-module.client";

export const expenses_setUserProject = createAction('[Expenses] Set UserProject', props<{ userProject: UserProjectResponse }>());
export const expenses_setUserProjects = createAction('[Expenses] Set UserProjects', props<{ userProjects: FilteredListResponseOfUserProjectResponse }>());
export const expenses_clearUserProjects = createAction('[Expenses] Clear UserProjects');
export const expenses_setUserAllowedProjects = createAction('[Expenses] Set UserAllowedProjects', props<{ userAllowedProjects: FilteredListResponseOfUserAllowedProjectResponse }>());
export const expenses_clearUserAllowedProjects = createAction('[Expenses] Clear UserAllowedProjects');
export const expenses_clearAll = createAction('[Expenses] Clear All');