import {Component, EventEmitter, Input, Output} from '@angular/core';
import {MatPaginatorModule, PageEvent} from "@angular/material/paginator";
import {CommonModule} from "@angular/common";
import {PaginatorEntity} from "../../../core/api-models/common.models";

@Component({
  selector: 'app-common-paginator',
  standalone: true,
  imports: [
    MatPaginatorModule,
    CommonModule
  ],
  templateUrl: './common-paginator.component.html',
  styleUrl: './common-paginator.component.scss'
})
export class CommonPaginatorComponent {
  @Input() paginator: PaginatorEntity | undefined;
  @Input() totalCount: number | undefined;
  @Input() pageSizeOptions: number[] = [1, 5, 10, 20, 50, 100];
  @Output() pageChanged: EventEmitter<PaginatorEntity> = new EventEmitter<PaginatorEntity>();

  handlePageEvent(e: PageEvent) {
    if (!this.paginator) {
      this.paginator = new PaginatorEntity({pageSize: e.pageSize, pageNumber: e.pageIndex + 1, isFull: false});
    } else {
      this.paginator.pageSize = e.pageSize;
      this.paginator.pageNumber = e.pageIndex + 1;
    }

    this.pageChanged.emit(this.paginator);
  }
}
