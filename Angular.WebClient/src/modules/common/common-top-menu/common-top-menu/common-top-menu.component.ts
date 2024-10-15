import {Component, EventEmitter, Input, Output} from '@angular/core';
import {MenuModel} from "../../../../core/models/common/menu.model";
import {CommonModule} from "@angular/common";
import {MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {CommonLoaderComponent} from "../../common-loader/common-loader.component";
import {RouterLink} from "@angular/router";
import {SharedModule} from "../../../../core/shared.module";

type InputType =
    'menuUserProject' |
    undefined;

@Component({
  selector: 'app-common-top-menu',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    CommonLoaderComponent,
    RouterLink,
    SharedModule,
  ],
  templateUrl: './common-top-menu.component.html',
  styleUrl: './common-top-menu.component.scss'
})
export class CommonTopMenuComponent {
  @Input() type: InputType;

  @Output() activeTabChanged: EventEmitter<number> = new EventEmitter<number>();

  private _menu: MenuModel | undefined

  get menu(): MenuModel {
    if (!this._menu) {
      this._menu = new MenuModel();
      if (this.type === 'menuUserProject') {
        this._menu.createUserProjectMenu(0);
      }
    }
    return this._menu;
  }

  get currentMenuTitle(): string {
    return this.menu?.menuItems?.[this.activeTab]?.title ?? '';
  }

  get activeTab(): number {
    return this.menu.activeTab ?? 0;
  }

  set activeTab(value: number) {
    this.menu.activeTab = value;
  }

  public mobileTabChanged(shift: number): void {
    if (this.activeTab + shift >= 0 && this.activeTab + shift <= 2) {
      this.tabChanged(this.activeTab + shift);
    }
  }

  public tabChanged(activeTab: number): void {
    this.activeTab = activeTab;
    this.activeTabChanged.emit(this.activeTab);
  }
}
