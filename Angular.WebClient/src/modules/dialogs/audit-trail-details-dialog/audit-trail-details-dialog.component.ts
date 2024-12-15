import {Component, Inject} from '@angular/core';
import {CommonModule} from "@angular/common";
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {CommonLoaderComponent} from "../../common/common-loader/common-loader.component";
import {AppCommonInputModule} from "../../common/common-input/app-common-input.module";
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../../core/shared.module";
import {MatSnackBar} from "@angular/material/snack-bar";
import {CommonDialogService} from "../../../core/services/common-dialog.service";
import {AuditTrailResponse, UserResponse} from "../../../core/api-models/common.models";
import {BaseUnsubscribeComponent} from "../../../core/base-components/base-unsubscribe.compoinent";

@Component({
    selector: 'app-audit-trail-details-dialog',
    imports: [
        CommonModule,
        MatDialogModule,
        MatButtonModule,
        CommonLoaderComponent,
        AppCommonInputModule,
        RouterLink,
        SharedModule,
    ],
    templateUrl: './audit-trail-details-dialog.component.html',
    styleUrl: './audit-trail-details-dialog.component.scss'
})
export class AuditTrailDetailsDialogComponent extends BaseUnsubscribeComponent{
  public auditTrail: AuditTrailResponse | undefined;

  constructor(
      public dialogRef: MatDialogRef<AuditTrailDetailsDialogComponent>,
      @Inject(MAT_DIALOG_DATA) public data: {
        auditTrail: AuditTrailResponse | undefined
      } | undefined,
      private readonly snackBar: MatSnackBar,
      private readonly commonDialogService: CommonDialogService
  ) {
    super();
    this.auditTrail = data?.auditTrail;
  }

  public openInitiatorInfoDialog(): void {
    if (!this.auditTrail?.userId) {
      this.snackBar.open('No user found', 'Close', {duration: 3000});
    }
    this._openUserInfoDialog();
  }

  public openEntityInfoDialog(): void {
    if (!this.auditTrail?.entityType) {
      this.snackBar.open('No entity found', 'Close', {duration: 3000});
    }
  }

  private _openUserInfoDialog(): void {

  }
}
