import { ActionReducerMap } from '@ngrx/store';
import {authReducer, AuthState} from "./auth.reducer";

export interface AppState {
  auth: AuthState;
}

export const reducers: ActionReducerMap<AppState> = {
  auth: authReducer,
};
