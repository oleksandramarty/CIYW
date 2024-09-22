import { IPaginatorModel, PaginatorModel } from "./paginator.model";

export interface IListWithIncludeModel<T> {
  entities: T[] | undefined;
  paginator: IPaginatorModel | undefined;
  totalCount: number | undefined;
}

export class ListWithIncludeModel<T> implements IListWithIncludeModel<T> {
  entities: T[] | undefined;
  paginator: PaginatorModel | undefined;
  totalCount: number | undefined;

  constructor(
    entities: T[] | undefined,
    paginator: PaginatorModel | undefined,
    totalCount: number | undefined) {
    this.entities = entities;
    this.paginator = paginator;
    this.totalCount = totalCount;
  }
}
