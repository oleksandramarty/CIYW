import {Component, HostListener, ViewChild, ElementRef} from '@angular/core';
import {MenuModel} from "../../../../core/models/common/menu.model";
import {UserResponse, UserRoleEnum} from "../../../../core/api-models/common.models";
import {Store} from "@ngrx/store";
import {MatSnackBar} from "@angular/material/snack-bar";
import {AuthService} from "../../../../core/services/auth.service";
import {selectUser} from "../../../../core/store/selectors/auth.selectors";
import {tap} from "rxjs";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {BaseAuthorizeComponent} from "../../../../core/base-components/base-authorize.component";
import {Router} from "@angular/router";

@Component({
    selector: 'app-common-quick-actions',
    templateUrl: './common-quick-actions.component.html',
    styleUrls: ['./common-quick-actions.component.scss'],
    standalone: false
})
export class CommonQuickActionsComponent extends BaseAuthorizeComponent {
    public quickActionsOpen: boolean = false;

    @ViewChild('quickActionsContainer') quickActionsContainer!: ElementRef;
    @ViewChild('icon') icon!: ElementRef;

    @HostListener('document:click', ['$event'])
    onDocumentClick(event: MouseEvent) {
        const target = event.target as HTMLElement;
        if (this.quickActionsContainer && !this.quickActionsContainer.nativeElement.contains(target) && this.quickActionsOpen) {
            this.toggleQuickActions();
        }
    }

    public currentUser: UserResponse | undefined;
    public quickActionsMenu: MenuModel | undefined;

    constructor(
        protected override readonly store: Store,
        protected override readonly snackBar: MatSnackBar,
        protected override readonly authService: AuthService,
        private readonly router: Router
    ) {
        super(authService, store, snackBar);

        this.store.select(selectUser)
            .pipe(
                tap((user) => {
                    if (!!user) {
                        this.currentUser = user;
                        this.quickActionsMenu = new MenuModel();
                        this.quickActionsMenu.createQuickActionsMenu(this.currentUser.roles.findIndex(role =>
                            role.id === UserRoleEnum.Admin ||
                            role.id === UserRoleEnum.SuperAdmin ||
                            role.id === UserRoleEnum.TechnicalSupport) > -1,
                            this._printCurrentPageAction.bind(this)
                        );
                    }
                }),
                handleApiError(this.snackBar)
            ).subscribe();
    }

    public goto(url: string | undefined): void {
        this.router.navigate([`/${url ?? ''}`]);
    }

    public toggleQuickActions() {
        this.quickActionsOpen = !this.quickActionsOpen;
        const icon = this.icon.nativeElement;
        icon.classList.add('rotate');
        setTimeout(() => {
            icon.classList.remove('rotate');
        }, 500);
    }

    private _printCurrentPageAction() {
        setTimeout(() => {
            window.print();
        }, 300);
    }
}