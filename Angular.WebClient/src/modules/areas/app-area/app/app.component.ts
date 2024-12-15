import {Component} from '@angular/core';
import {AuthService} from "../../../../core/services/auth.service";
import {BaseInitializationService} from "../../../../core/services/base-initialization.service";
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss'],
    standalone: false
})
export class AppComponent extends BaseAuthorizeComponent {
    constructor(
        protected override readonly authService: AuthService,
        protected override readonly store: Store,
        protected override readonly snackBar: MatSnackBar,
        private readonly baseInitializationService: BaseInitializationService
    ) {
        super(authService, store, snackBar);
        this.authService.initialize();
        this.baseInitializationService.initialize();
    }
}