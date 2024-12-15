import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject} from "rxjs";
import {DictionaryMap} from "../../../../../core/models/common/dictionary.model";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {UserProjectsService} from "../../../../../core/services/entity-services/user-projects.service";
import {
  CurrencyResponse,
  UserAllowedProjectResponse,
  UserProjectResponse
} from "../../../../../core/api-models/common.models";
import {BaseUnsubscribeComponent} from "../../../../../core/base-components/base-unsubscribe.compoinent";

@Component({
    selector: 'app-dashboard-user-projects',
    templateUrl: './dashboard-user-projects.component.html',
    styleUrl: './dashboard-user-projects.component.scss',
    standalone: false
})
export class DashboardUserProjectsComponent extends BaseUnsubscribeComponent {
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
    super();
  }

  override ngOnInit(): void {
    this.userProjectsService.initProjects(this.ngUnsubscribe);
  }

  public openUserProject(id: string | undefined): void {
    this.userProjectsService.openUserProject(id);
  }

  public openCreateUserProjectDialog(): void {
    this.userProjectsService.openCreateUserProjectDialog(this.ngUnsubscribe);
  }
}
