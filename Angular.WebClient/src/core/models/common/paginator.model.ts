export interface IPaginatorModel {
  pageNumber?: number | undefined;
  pageSize?: number | undefined;
  isFull?: boolean | undefined;
}

export class PaginatorModel implements IPaginatorModel {
  pageNumber?: number | undefined;
  pageSize?: number | undefined;
  isFull?: boolean | undefined;

  constructor(pageNumber?: number, pageSize?: number, isFull?: boolean) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.isFull = isFull;
  }
}
