import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject} from "rxjs";
import {DictionaryMap} from "../../../../../core/models/common/dictionarie.model";
import {CurrencyResponse} from "../../../../../core/api-clients/dictionaries-client";
import {UserAllowedProjectResponse, UserProjectResponse} from "../../../../../core/api-clients/expenses-client";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {UserProjectsService} from "../../../../../core/services/entity-services/user-projects.service";

@Component({
  selector: 'app-expense-dashboard-projects',
  templateUrl: './expense-dashboard-projects.component.html',
  styleUrl: './expense-dashboard-projects.component.scss'
})
export class ExpenseDashboardProjectsComponent implements OnInit, OnDestroy{
  protected ngUnsubscribe: Subject<void> = new Subject<void>();

  get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
    return this.dictionaryService.currenciesMap;
  }

  get userProjects(): UserProjectResponse[] | undefined {
    return this.userProjectsService.userProjects;
  }

  get userAllowedProjects(): UserAllowedProjectResponse[] | undefined {
    return this.userProjectsService.userAllowedProjects;
  }

  constructor(
      private readonly dictionaryService: DictionaryService,
      private readonly userProjectsService: UserProjectsService,
  ) {
  }

  public ngOnInit(): void {
    this.userProjectsService.initProjects(this.ngUnsubscribe);
  }

  public ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  public openUserProject(id: string | undefined): void {
    this.userProjectsService.openUserProject(id);
  }

  public openCreateUserProjectDialog(): void {
    this.userProjectsService.openCreateUserProjectDialog(this.ngUnsubscribe);
  }
}
