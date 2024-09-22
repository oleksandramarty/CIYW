export interface IBaseSortableModel {
  column?: string | undefined;
  direction?: string | undefined;
}

export class BaseSortableModel implements IBaseSortableModel {
  column?: string | undefined;
  direction?: string | undefined;

  constructor(column?: string, direction?: string) {
    this.column = column;
    this.direction = direction;
  }
}
