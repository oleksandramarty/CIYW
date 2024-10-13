import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from "@angular/material/dialog";
import { MatButtonModule } from "@angular/material/button";
import { Subject, takeUntil, tap } from "rxjs";
import { MatSnackBar } from "@angular/material/snack-bar";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { CommonLoaderComponent } from "../../common/common-loader/common-loader.component";
import { AppCommonInputModule } from "../../common/common-input/app-common-input.module";
import { CommonModule } from "@angular/common";
import { RouterLink } from "@angular/router";
import { LocalizationService } from "../../../core/services/localization.service";
import { handleApiError } from "../../../core/helpers/rxjs.helper";
import { SharedModule } from "../../../core/shared.module";
import {LoaderService} from "../../../core/services/loader.service";
import {CommonDialogService} from "../../../core/services/common-dialog.service";
import {GraphQlExpensesService} from "../../../core/graph-ql/services/graph-ql-expenses.service";
import {UserProjectResponse} from "../../../core/api-models/common.models";

@Component({
  selector: 'app-create-update-user-project',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    CommonLoaderComponent,
    AppCommonInputModule,
    RouterLink,
    SharedModule,
  ],
  templateUrl: './create-update-user-project.component.html',
  styleUrls: ['./create-update-user-project.component.scss'] // Corrected property name
})
export class CreateUpdateUserProjectComponent implements OnInit, OnDestroy {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();

  public userProject: UserProjectResponse | undefined;
  public userProjectForm: FormGroup | undefined;

  constructor(
    public dialogRef: MatDialogRef<CreateUpdateUserProjectComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      userProject: UserProjectResponse | undefined
    },
    private readonly snackBar: MatSnackBar,
    private readonly fb: FormBuilder,
    private readonly localizationService: LocalizationService,
    private readonly graphQlExpensesService: GraphQlExpensesService,
    private readonly loaderService: LoaderService,
    private readonly commonDialogService: CommonDialogService,
  ) {
    this.userProject = data.userProject;
  }

  ngOnInit(): void {
    this.createUserForm();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  private createUserForm() {
    this.userProjectForm = this.fb.group({
      title: [this.userProject?.title, [Validators.required]],
      isActive: [this.userProject?.isActive ?? true, [Validators.required]]
    });
  }

  public createOrUpdateUserProject() {
    if (!this.userProjectForm?.valid) {
      Object.keys(this.userProjectForm?.controls ?? {}).forEach(key => {
        const control = this.userProjectForm?.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
      this.snackBar.open(this.localizationService?.getTranslation('ERROR.FORM_VALIDATION') ?? 'ERROR', 'Close', { duration: 3000 });
      return;
    }

    const createUserProjectAction = () => {
      if (!this.userProjectForm) {
        return;
      }

      this.loaderService.isBusy = true;

      this.graphQlExpensesService.createUserProject(
          this.userProjectForm.value.title,
          this.userProjectForm.value.isActive)
          .pipe(
              takeUntil(this.ngUnsubscribe),
              tap(() => {
                this.snackBar.open(this.localizationService?.getTranslation('SUCCESS') ?? '', 'Close', { duration: 3000 });
                this.loaderService.isBusy = false;
                this.dialogRef.close(true);
              }),
              handleApiError(this.snackBar)
          ).subscribe();
    }

    this.commonDialogService.showNoComplaintModal(createUserProjectAction)
  }
}