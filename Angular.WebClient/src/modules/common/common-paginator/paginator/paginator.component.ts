import {Component, Input} from '@angular/core';
import {PaginatorModel} from "../../../../core/models/common/paginator.model";

@Component({
  selector: 'app-paginator',
  templateUrl: './paginator.component.html',
  styleUrl: './paginator.component.scss'
})
export class PaginatorComponent {
@Input() paginator: PaginatorModel | undefined;
@Input() totalCount: number | undefined;
}
