import {Component, EventEmitter, Input, OnDestroy, OnInit, Output} from '@angular/core';
import {Subject} from "rxjs";
import {
  BalanceResponse,
  CurrencyResponse,
  IconResponse,
  UserProjectResponse
} from "../../../../../core/api-models/common.models";
import {DictionaryMap} from "../../../../../core/models/common/dictionary.model";
import {DictionaryService} from "../../../../../core/services/dictionary.service";
import {CommonDialogService} from "../../../../../core/services/common-dialog.service";

@Component({
  selector: 'app-user-project-favorites',
  templateUrl: './user-project-favorites.component.html',
  styleUrl: '../user-project.component.scss'
})
export class UserProjectFavoritesComponent implements OnInit, OnDestroy {
  protected ngUnsubscribe: Subject<void> = new Subject<void>();
  @Input() userProject: UserProjectResponse | undefined;
  @Output() favoritesChanged: EventEmitter<void> = new EventEmitter();

  get currenciesMap(): DictionaryMap<number, CurrencyResponse> | undefined {
    return this.dictionaryService.currenciesMap;
  }

  get iconMap(): DictionaryMap<number, IconResponse> | undefined {
    return this.dictionaryService.iconMap;
  }

  constructor(
      private readonly dictionaryService: DictionaryService,
      private readonly commonDialogService: CommonDialogService
  ) {
  }

  public ngOnInit(): void {
  }

  public ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  public openBalanceDialog(balance: BalanceResponse | undefined): void {
    this.commonDialogService.showCreateOrUpdateUserBalanceModal(() => {
      this.favoritesChanged.emit();
    }, balance, this.userProject);
  }
}
