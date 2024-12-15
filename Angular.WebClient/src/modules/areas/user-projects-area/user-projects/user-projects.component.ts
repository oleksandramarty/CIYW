import {Component, OnDestroy, OnInit} from '@angular/core';
import {DictionaryService} from "../../../../core/services/dictionary.service";
import {DictionaryMap} from "../../../../core/models/common/dictionary.model";
import {UserProjectsService} from "../../../../core/services/entity-services/user-projects.service";
import {Subject} from "rxjs";
import {
    BalanceTypeResponse,
    CurrencyResponse,
    UserAllowedProjectResponse,
    UserProjectResponse
} from "../../../../core/api-models/common.models";
import {BaseUnsubscribeComponent} from "../../../../core/base-components/base-unsubscribe.compoinent";

@Component({
    selector: 'app-user-projects',
    templateUrl: './user-projects.component.html',
    styleUrl: './user-projects.component.scss',
    standalone: false
})
export class UserProjectsComponent extends BaseUnsubscribeComponent {
    get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
        return this.dictionaryService.currenciesMap;
    }
    get balanceTypesMap(): DictionaryMap<number, BalanceTypeResponse> | undefined {
        return this.dictionaryService.balanceTypesMap;
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
