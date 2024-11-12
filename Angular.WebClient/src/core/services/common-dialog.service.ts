import {Injectable} from "@angular/core";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {ConfirmationMessageDialogComponent} from "../../modules/dialogs/confirmation-message-dialog/confirmation-message-dialog.component";
import {Observable, take, takeUntil} from "rxjs";
import {tap} from "rxjs/operators";
import {handleApiError} from "../helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "./localization.service";
import {
    CreateUpdateExpenseDialogComponent
} from "../../modules/dialogs/create-update-expense-dialog/create-update-expense-dialog.component";
import {
    CreateUpdatePlannedExpenseDialogComponent
} from "../../modules/dialogs/create-update-planned-expense-dialog/create-update-planned-expense-dialog.component";
import {
    AuditTrailResponse,
    BalanceResponse,
    ExpenseResponse, FavoriteExpenseResponse,
    PlannedExpenseResponse,
    UserProjectResponse
} from "../api-models/common.models";
import {
    CreateUpdateBalanceDialogComponent
} from "../../modules/dialogs/create-update-balance-dialog/create-update-balance-dialog.component";
import {IconPickerDialogComponent} from "../../modules/dialogs/icon-picker-dialog/icon-picker-dialog.component";
import {
    CreateUpdateFavoriteExpenseDialogComponent
} from "../../modules/dialogs/create-update-favorite-expense-dialog/create-update-favorite-expense-dialog.component";
import {
    AuditTrailDetailsDialogComponent
} from "../../modules/dialogs/audit-trail-details-dialog/audit-trail-details-dialog.component";

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

    public showNoComplaintDialog(executableAction: () => void, executableCancelAction: () => void): void {
        this._handeExecutableAction<ConfirmationMessageDialogComponent>(this._getNoComplaintDialog(), executableAction, executableCancelAction);
    }

    private _getNoComplaintDialog(): MatDialogRef<ConfirmationMessageDialogComponent, any> {
        return this.dialog.open(ConfirmationMessageDialogComponent, {
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

    public showRemoveExpenseConfirmationDialog(executableAction: () => void, executableCancelAction: () => void): void {
        this._handeExecutableAction<ConfirmationMessageDialogComponent>(this._getRemoveExpenseConfirmationDialog(), executableAction, executableCancelAction);
    }

    private _getRemoveExpenseConfirmationDialog(): MatDialogRef<ConfirmationMessageDialogComponent, any> {
        return this._getConfirmationDialog();
    }

    private _getConfirmationDialog(
        title: string = 'DIALOG.DELETE_TITLE',
        descriptions: string[] = ['DIALOG.DELETE_DESCRIPTION'],
        yesBtn: string = 'COMMON.YES',
        noBtn: string = 'COMMON.NO'): MatDialogRef<ConfirmationMessageDialogComponent, any> {
        return this.dialog.open(ConfirmationMessageDialogComponent, {
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

    private _handeExecutableAction<TDialogRef>(dialogRef: MatDialogRef<TDialogRef, any>, executableAction: () => void, executableCancelAction: () => void): void {
        dialogRef.afterClosed()
            .pipe(
                take(1),
                tap((result) => {
                    if (result) {
                        executableAction();
                    } else {
                        executableCancelAction();
                    }
                }),
                handleApiError(this.snackBar)
            )
            .subscribe();
    }

    public showCreateOrUpdateExpenseDialog(executableAction: () => void, executableCancelAction: () => void, expense: ExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateExpenseDialogComponent>(this._getCreateUpdateExpenseDialog(expense, userProject), executableAction, executableCancelAction);
    }

    public showCreateOrUpdateExpenseByFavoriteDialog(executableAction: () => void, executableCancelAction: () => void, balance: BalanceResponse | undefined, favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateExpenseDialogComponent>(this._getCreateUpdateExpenseByFavoriteDialog(balance, favoriteExpense, userProject), executableAction, executableCancelAction);
    }

    public showCreateOrUpdatePlannedExpenseDialog(executableAction: () => void, executableCancelAction: () => void, plannedExpense: PlannedExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdatePlannedExpenseDialogComponent>(this._getCreateUpdatePlannedExpenseDialog(plannedExpense, userProject), executableAction, executableCancelAction);
    }

    public showCreateOrUpdateUserBalanceDialog(executableAction: () => void, executableCancelAction: () => void, balance: BalanceResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateBalanceDialogComponent>(this._getCreateUpdateUserBalanceDialog(balance, userProject), executableAction, executableCancelAction);
    }

    public showCreateOrUpdateFavoriteExpenseDialog(executableAction: () => void, executableCancelAction: () => void, favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): void {
        this._handeExecutableAction<CreateUpdateFavoriteExpenseDialogComponent>(this._getCreateUpdateFavoriteExpenseDialog(favoriteExpense, userProject), executableAction, executableCancelAction);
    }

    public showIconPickerDialog(): Observable<any> {
        return this._getIconPickerDialog().afterClosed();
    }

    public showAuditTrailDetailsDialog(executableAction: () => void, executableCancelAction: () => void, auditTrails: AuditTrailResponse | undefined): void {
        this._handeExecutableAction<AuditTrailDetailsDialogComponent>(this._getAuditTrailDetailsDialog(auditTrails), executableAction, executableCancelAction);
    }

    private _getCreateUpdateExpenseDialog(expense: ExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateExpenseDialogComponent, any> {
        return this.dialog.open(CreateUpdateExpenseDialogComponent, {
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

    private _getCreateUpdateExpenseByFavoriteDialog(balance: BalanceResponse | undefined, favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateExpenseDialogComponent, any> {
        return this.dialog.open(CreateUpdateExpenseDialogComponent, {
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

    private _getCreateUpdatePlannedExpenseDialog(plannedExpense: PlannedExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdatePlannedExpenseDialogComponent, any> {
        return this.dialog.open(CreateUpdatePlannedExpenseDialogComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                plannedExpense,
                userProject
            }
        });
    }

    private _getCreateUpdateUserBalanceDialog(balance: BalanceResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateBalanceDialogComponent, any> {
        return this.dialog.open(CreateUpdateBalanceDialogComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                balance,
                userProject
            }
        });
    }

    private _getCreateUpdateFavoriteExpenseDialog(favoriteExpense: FavoriteExpenseResponse | undefined, userProject: UserProjectResponse | undefined): MatDialogRef<CreateUpdateFavoriteExpenseDialogComponent, any> {
        return this.dialog.open(CreateUpdateFavoriteExpenseDialogComponent, {
            width: '600px',
            maxWidth: '80vw',
            data: {
                favoriteExpense,
                userProject
            }
        });
    }

    private _getIconPickerDialog(): MatDialogRef<IconPickerDialogComponent, any> {
        return this.dialog.open(IconPickerDialogComponent, {
            width: '800px',
            maxWidth: '80vw',
            data: {}
        });
    }

    private _getAuditTrailDetailsDialog(auditTrail: AuditTrailResponse | undefined): MatDialogRef<AuditTrailDetailsDialogComponent, any> {
        return this.dialog.open(AuditTrailDetailsDialogComponent, {
            width: auditTrail?.message?.length ?? 0 > 400 ? '90vw' : '600px',
            maxWidth: auditTrail?.message?.length ?? 0 > 400 ? '90vw' : '80vw',
            data: {
                auditTrail
            }
        });
    }
}
