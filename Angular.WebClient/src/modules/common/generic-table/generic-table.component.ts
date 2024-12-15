import {Component, EventEmitter, Input, Output} from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {SharedModule} from "../../../core/shared.module";
import {MatDividerModule} from "@angular/material/divider";
import {MatTooltipModule} from "@angular/material/tooltip";
import {PaginatorEntity} from "../../../core/api-models/common.models";
import {CommonPaginatorComponent} from "../common-paginator/common-paginator.component";

@Component({
    selector: 'app-generic-table',
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        SharedModule,
        MatDividerModule,
        MatTooltipModule,
        CommonPaginatorComponent
    ],
    templateUrl: './generic-table.component.html',
    styleUrl: './generic-table.component.scss'
})
export class GenericTableComponent {
  @Input() public totalCount: number | undefined;
  @Input() public paginator: PaginatorEntity | undefined;
  @Output() public pageChanged: EventEmitter<PaginatorEntity> = new EventEmitter<PaginatorEntity>();
}
