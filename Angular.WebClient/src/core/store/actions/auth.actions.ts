import { createAction, props } from '@ngrx/store';
import {JwtTokenResponse} from "../../models/auth/account.model";
import {UserResponse} from "../../models/users/user.model";

export const setToken = createAction('[Auth] Set Token', props<{ token: JwtTokenResponse }>());
export const clearToken = createAction('[Auth] Clear Token');
export const setUser = createAction('[Auth] Set User', props<{ user: UserResponse }>());
export const clearUser = createAction('[Auth] Clear User');
export const clearAll = createAction('[Auth] Clear All');
