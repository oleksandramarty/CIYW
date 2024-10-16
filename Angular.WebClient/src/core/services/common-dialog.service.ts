import {Injectable} from "@angular/core";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {ConfirmationMessageComponent} from "../../modules/dialogs/confirmation-message/confirmation-message.component";
import {Observable, take, takeUntil} from "rxjs";
import {tap} from "rxjs/operators";
import {handleApiError} from "../helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "./localization.service";
import {
    CreateUpdateExpenseComponent
} from "../../modules/dialogs/create-update-expense/create-update-expense.component";
import {
    CreateUpdatePlannedExpenseComponent
} from "../../modules/dialogs/create-update-planned-expense/create-update-planned-expense.component";
import {
    BalanceResponse,
    ExpenseResponse, FavoriteExpenseResponse,
    PlannedExpenseResponse,
    UserProjectResponse
} from "../api-models/common.models";
import {
    CreateUpdateBalanceComponent
} from "../../modules/dialogs/create-update-balance/create-update-balance.component";
import {IconPickerComponent} from "../../modules/dialogs/icon-picker/icon-picker.component";
import {
    CreateUpdateFavoriteExpenseComponent
} from "../../modules/dialogs/create-update-favorite-expense/create-update-favorite-expense.component";

@Injectable({
    providedIn: 'root'
})
export class CommonDialogService {
    constructor(
        private readonly dialog: MatDialog,
        private readonly snackBar: MatSnackBar,
        private readonly localizationService: LocalizationService
    ) {
    }

    public showNoComplaintModal(executableAction: () => void): void {
        this._handeExecutableAction<ConfirmationMessageComponent>(this._getNoComplaintModal(), executableAction);
    }

    private _getNoComplaintModal(): MatDialogRef<ConfirmationMessageComponent, any> {
        return this.dialog.open(ConfirmationMessageComponent, {
            width: '400px',
            maxWidth: '80vw',
            data: {
                yesBtn: 'COMMON.PROCEED',
                noBtn: 'COMMON.CANCEL',
                title: 'COMMON.WARNING',
                htmlBlock: `
        <h2 style="color: #dc3545; text-align: center;">${this.localizationService.getTranslation('COMMON.DO_NOT_STORE_ANY_SENSITIVE_DATA_HERE')}</h2>
        <p style="text-align: center"><u>${this.localizationService.getTranslation('AUTH.NO_COMPLAINTS')}</u></p>
        `
            }
        });
    }

    public showRemoveExpenseConfirmationModal(executableAction: () => void): void {
        this._handeExecutableAction<ConfirmationMessageComponent>(this._getRemoveExpenseConfirmationModal(), executableAction);
    }

    private _getRemoveExpenseConfirmationModal(): MatDialogRef<ConfirmationMessageComponent, any> {
        return this._getConfirmationModal();
    }

    private _getConfirmationModal(
        title: string = 'DIALOG.DELETE_TITLE',
        descriptions: string[] = ['DIALOG.DELETE_DESCRIPTION'],
        yesBtn: string = 'COMMON.YES',
        noBtn: string = 'COMMON.NO'): MatDialogRef<ConfirmationMessageComponent, any> {
        return this.dialog.open(ConfirmationMessageComponent, {
            width: '400px',
            maxWidth: '80vw',
            data: {
                yesBtn,
                noBtn,
                title,
                descriptions
            }
        });
    }

    private _handeExecutableAction<TDialogRef>(dialogRef: MatDialogRef<TDialogRef, any>, executableAction: () => void): void {
        dialogRef.afterClosed()
            .pipe(
                take(1),
                tap((result) => {
                    if (result) {
                        executableAction();
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    public showCreateOrUpdateExpenseModal(executableAction: () => void, expense: ExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateExpenseComponent>(this._getCreateUpdateExpenseModal(expense, userProject), executableAction);
    }

    public showCreateOrUpdateExpenseByFavoriteModal(executableAction: () => void, balance: BalanceResponse | undefined, favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateExpenseComponent>(this._getCreateUpdateExpenseByFavoriteModal(balance, favoriteExpense, userProject), executableAction);
    }

    public showCreateOrUpdatePlannedExpenseModal(executableAction: () => void, plannedExpense: PlannedExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdatePlannedExpenseComponent>(this._getCreateUpdatePlannedExpenseModal(plannedExpense, userProject), executableAction);
    }

    public showCreateOrUpdateUserBalanceModal(executableAction: () => void, balance: BalanceResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateBalanceComponent>(this._getCreateUpdateUserBalanceModal(balance, userProject), executableAction);
    }

    public showCreateOrUpdateFavoriteExpenseModal(executableAction: () => void, favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateFavoriteExpenseComponent>(this._getCreateUpdateFavoriteExpenseModal(favoriteExpense, userProject), executableAction);
    }

    public showIconPickerModal(): Observable<any> {
        return this._getIconPickerModal().afterClosed();
    }

    private _getCreateUpdateExpenseModal(expense: ExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateExpenseComponent, any> {
        return this.dialog.open(CreateUpdateExpenseComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                expense,
                userProject,
                balance: undefined,
                favoriteExpense: undefined
            }
        });
    }

    private _getCreateUpdateExpenseByFavoriteModal(balance: BalanceResponse | undefined, favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateExpenseComponent, any> {
        return this.dialog.open(CreateUpdateExpenseComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                expense: undefined,
                userProject,
                balance,
                favoriteExpense
            }
        });
    }

    private _getCreateUpdatePlannedExpenseModal(plannedExpense: PlannedExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdatePlannedExpenseComponent, any> {
        return this.dialog.open(CreateUpdatePlannedExpenseComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                plannedExpense,
                userProject
            }
        });
    }

    private _getCreateUpdateUserBalanceModal(balance: BalanceResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateBalanceComponent, any> {
        return this.dialog.open(CreateUpdateBalanceComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                balance,
                userProject
            }
        });
    }

    private _getCreateUpdateFavoriteExpenseModal(favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateFavoriteExpenseComponent, any> {
        return this.dialog.open(CreateUpdateFavoriteExpenseComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                favoriteExpense,
                userProject
            }
        });
    }

    private _getIconPickerModal(): MatDialogRef<IconPickerComponent, any> {
        return this.dialog.open(IconPickerComponent, {
            width: '800px',
            maxWidth: '80vw',
            data: {}
        });
    }
}
