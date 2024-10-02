import {Component, OnDestroy, OnInit} from '@angular/core';
import {finalize, Subject, switchMap, takeUntil, tap} from "rxjs";
import {handleApiError} from "../../../../core/helpers/rxjs.helper";
import {
  ExpenseClient,
  UserAllowedProjectResponse,
  UserProjectResponse
} from "../../../../core/api-clients/expenses-client";
import {MatSnackBar} from "@angular/material/snack-bar";
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {DictionaryMap} from "../../../../core/models/common/dictionarie.model";
import {LocaleResponse} from "../../../../core/api-clients/localizations-client";
import {MatDialog} from "@angular/material/dialog";
import {
    CreateUpdateUserProjectComponent
} from "../../../dialogs/create-update-user-project/create-update-user-project.component";

@Component({
  selector: 'app-user-projects',
  templateUrl: './user-projects.component.html',
  styleUrl: './user-projects.component.scss'
})
export class UserProjectsComponent implements OnInit, OnDestroy {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  userProjects: UserProjectResponse[] | undefined;
  allowedProjects: UserAllowedProjectResponse[] | undefined;
  isBusy: boolean = true;

  get localesMap(): DictionaryMap<number, LocaleResponse> | undefined {
      return this.dictionaryService.localesMap;
  }

  constructor(
      private readonly dictionaryService: DictionaryService,
      private readonly expenseClient: ExpenseClient,
      private snackBar: MatSnackBar,
      private dialog: MatDialog
  ) {
  }

  public ngOnInit(): void {
    this.getUserProjects();
  }

  public ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  public openCreateUserProjectDialog(): void {
      const dialogRef = this.dialog.open(CreateUpdateUserProjectComponent, {
          width: '400px',
          maxWidth: '80vw',
          data: {}
      });

      dialogRef.afterClosed().subscribe(result => {
          console.log(result);
      });
  }

  private getUserProjects(): void {
    this.isBusy = true;
    this.expenseClient.userProject_GetProjects()
        .pipe(
            takeUntil(this.ngUnsubscribe),
            switchMap(userProjects => {
              this.userProjects = userProjects;
              return this.expenseClient.userProject_GetAllowedProjects();
            }),
            tap(allowedProjects => {
              this.allowedProjects = allowedProjects;
            }),
            handleApiError(this.snackBar),
            finalize(() => this.isBusy = false)
        ).subscribe();
  }
}
