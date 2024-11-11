import {createFeatureSelector, createSelector} from "@ngrx/store";
import {SiteState} from "../reducers/site.reducre";

export const selectSiteState = createFeatureSelector<SiteState>('site');

export const selectMenuState = createSelector(
    selectSiteState,
    (state: SiteState) => state.menu_open
);