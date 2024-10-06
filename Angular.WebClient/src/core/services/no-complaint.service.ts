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
export class NoComplaintService {
    constructor(
        private readonly dialog: MatDialog,
        private readonly snackBar: MatSnackBar,
        private readonly localizationService: LocalizationService
    ) {
    }

    public showNoComplaintModal(executableAction: () => void): void {
        let dialogRef = this.getNoComplaintModal();

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

    private getNoComplaintModal(): MatDialogRef<ConfirmationMessageComponent, any> {
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
}