import { createReducer, on } from '@ngrx/store';
import { setToken, clearToken, setUser, clearUser, clearAll } from '../actions/auth.actions';
import {JwtTokenResponse, UserResponse} from "../../api-clients/auth-client";

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
  on(setToken, (state, { token }) => ({ ...state, token })),
  on(clearToken, state => ({ ...state, token: undefined })),
  on(setUser, (state, { user }) => ({ ...state, user })),
  on(clearUser, state => ({ ...state, user: undefined })),
  on(clearAll, state => ({ ...state, token: undefined, user: undefined }))
);
