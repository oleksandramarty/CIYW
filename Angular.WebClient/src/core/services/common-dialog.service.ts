import {Injectable} from "@angular/core";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {ConfirmationMessageComponent} from "../../modules/dialogs/confirmation-message/confirmation-message.component";
import {take} from "rxjs";
import {tap} from "rxjs/operators";
import {handleApiError} from "../helpers/rxjs.helper";
import {MatSnackBar} from "@angular/material/snack-bar";
import {LocalizationService} from "./localization.service";

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
        this._handeExecutableAction(this._getNoComplaintModal(), executableAction);
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
        this._handeExecutableAction(this._getRemoveExpenseConfirmationModal(), executableAction);
    }

    private _getRemoveExpenseConfirmationModal(): MatDialogRef<ConfirmationMessageComponent, any> {
        return this._getConfirmationModal('EXPENSES.DELETE_EXPENSE', ['EXPENSES.DELETE_EXPENSE_CONFIRMATION']);
    }

    private _getConfirmationModal(
        title: string,
        descriptions: string[],
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

    private _handeExecutableAction(dialogRef: MatDialogRef<ConfirmationMessageComponent, any>, executableAction: () => void): void {
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
}
