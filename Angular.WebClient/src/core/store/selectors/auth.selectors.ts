import { createSelector, createFeatureSelector } from '@ngrx/store';
import { AuthState } from '../reducers/auth.reducer';
import {UserRoleEnum} from "../../api-models/common.models";

export const selectAuthState = createFeatureSelector<AuthState>('auth');

export const selectToken = createSelector(
  selectAuthState,
  (state: AuthState) => state.token
);

export const selectUser = createSelector(
  selectAuthState,
  (state: AuthState) => state.user
);

export const selectIsAdmin = createSelector(
  selectAuthState,
  (state: AuthState) => (state.user?.roles?.findIndex(role => role.id === UserRoleEnum.Admin) ?? -1) > -1
);

export const selectIsSuperAdmin = createSelector(
  selectAuthState,
  (state: AuthState) => (state.user?.roles?.findIndex(role => role.id === UserRoleEnum.SuperAdmin) ?? -1) > -1
);

export const selectIsTechnicalSupport = createSelector(
  selectAuthState,
  (state: AuthState) => (state.user?.roles?.findIndex(role => role.id === UserRoleEnum.TechnicalSupport) ?? -1) > -1
);

export const selectIsUser = createSelector(
  selectAuthState,
  (state: AuthState) => (state.user?.roles?.findIndex(role => role.id === UserRoleEnum.User) ?? -1) > -1
);


export const selectIsAdminAreaAvailable = createSelector(
  selectAuthState,
  (state: AuthState) => (state.user?.roles?.findIndex(role => [UserRoleEnum.Admin, UserRoleEnum.SuperAdmin, UserRoleEnum.TechnicalSupport].findIndex(x => x === role.id) > -1) ?? -1) > -1
);
