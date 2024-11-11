import {Directive, OnDestroy, OnInit} from "@angular/core";
import {Observable, Subject, tap} from "rxjs";
import {BaseUnsubscribeComponent} from "./base-unsubscribe.compoinent";
import {UserResponse} from "../api-models/common.models";
import {Store} from "@ngrx/store";
import {AuthService} from "../services/auth.service";
import {selectUser} from "../store/selectors/auth.selectors";
import {handleApiError} from "../helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";

@Directive()
export abstract class BaseAuthorizeComponent extends BaseUnsubscribeComponent {
    public currentUser: UserResponse | undefined;

    protected constructor(
        protected readonly authService: AuthService,
        protected readonly store: Store,
        protected readonly snackBar: MatSnackBar
    ) {
        super();
    }

    get isAuthorized$(): Observable<boolean> | undefined {
        return this.authService.isAuthorized$;
    }

    get isUser$(): Observable<boolean> | undefined {
        return this.authService.isUser$;
    }

    get isAdmin$(): Observable<boolean> | undefined {
        return this.authService.isAdmin$;
    }

    get isSuperAdmin$(): Observable<boolean> | undefined {
        return this.authService.isSuperAdmin$;
    }

    get isTechnicalSupport$(): Observable<boolean> | undefined {
        return this.authService.isTechnicalSupport$;
    }

    get isAdminAreaAvailable$(): Observable<boolean> | undefined {
        return this.authService.isAdminAreaAvailable$;
    }

    override ngOnInit(): void {
        this.store.select(selectUser)
            .pipe(
                tap((user) => {
                    this.currentUser = user;
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }
}
