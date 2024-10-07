import { createReducer, on } from '@ngrx/store';
import { auth_setToken, auth_clearToken, auth_setUser, auth_clearUser, auth_clearAll } from '../actions/auth.actions';
import {UserResponse} from "../../api-clients/common-module.client";
import {JwtTokenResponse} from "../../api-clients/common-module.client";

export interface AuthState {
  token: JwtTokenResponse | undefined;
  user: UserResponse | undefined;
}

export const initialState: AuthState = {
  token: undefined,
  user: undefined,
};

export const authReducer = createReducer(
  initialState,
  on(auth_setToken, (state, { token }) => ({ ...state, token })),
  on(auth_clearToken, state => ({ ...state, token: undefined })),
  on(auth_setUser, (state, { user }) => ({ ...state, user })),
  on(auth_clearUser, state => ({ ...state, user: undefined })),
  on(auth_clearAll, state => ({ ...state, token: undefined, user: undefined }))
);
