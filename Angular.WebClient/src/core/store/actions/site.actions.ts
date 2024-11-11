import {createAction, props} from "@ngrx/store";

export const menu_toggle = createAction('[Menu] Toggle');
export const menu_setMenuState = createAction('[Menu] Set State', props<{ menuState: boolean }>());