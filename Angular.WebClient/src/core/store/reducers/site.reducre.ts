import {createReducer, on} from "@ngrx/store";
import {menu_setMenuState, menu_toggle} from "../actions/site.actions";

export interface SiteState {
    menu_open: boolean | undefined;
}

export const initialState: SiteState = {
    menu_open: undefined
};

export const siteReducer = createReducer(
    initialState,
    on(menu_toggle, (state) => ({...state, menu_open: !state.menu_open})),
    on(menu_setMenuState, (state, {menuState}) => ({...state, menu_open: menuState}))
);