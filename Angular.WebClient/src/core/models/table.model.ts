import {ColumnEnum} from "../api-models/common.models";

export interface ITableHeaderItem {
  title: string | undefined;
  icon: string | undefined;
  isSortable: boolean | undefined;
  column: ColumnEnum | undefined;
}

export class TableHeaderItem implements ITableHeaderItem {
  title: string | undefined;
  icon: string | undefined;
  isSortable: boolean | undefined;
  column: ColumnEnum | undefined;

  constructor(
    title: string | undefined,
    icon: string | undefined,
    isSortable: boolean | undefined,
    column: ColumnEnum | undefined
  ) {
    this.title = title;
    this.icon = icon;
    this.isSortable = isSortable;
    this.column = column;
  }
}

export const createUserProjectExpensesHeader = (): ITableHeaderItem[] => {
  return [
    new TableHeaderItem('', undefined, false, undefined),
    new TableHeaderItem('', undefined, false, undefined),
    new TableHeaderItem('COMMON.DATE', undefined, true, ColumnEnum.Date),
    new TableHeaderItem('COMMON.COMMENT', undefined, false, undefined),
    new TableHeaderItem('COMMON.AMOUNT', undefined, true, ColumnEnum.Amount),
    new TableHeaderItem('COMMON.CATEGORY', undefined, false, undefined),
  ];
}

export const createAdminAuditTrailHeader = (): ITableHeaderItem[] => {
  return [
    new TableHeaderItem('', undefined, false, undefined),
    new TableHeaderItem('AUDIT_TRAIL.TYPE', undefined, false, undefined),
    new TableHeaderItem('AUDIT_TRAIL.ACTION', undefined, false, undefined),
    new TableHeaderItem('AUDIT_TRAIL.EXCEPTION_TYPE', undefined, false, undefined),
    new TableHeaderItem('AUDIT_TRAIL.MESSAGE', undefined, false, undefined),
    new TableHeaderItem('AUDIT_TRAIL.URI', undefined, false, undefined),
    new TableHeaderItem('AUDIT_TRAIL.ENTITY_TYPE', undefined, false, undefined),
    new TableHeaderItem('AUDIT_TRAIL.CREATED_AT', undefined, true, ColumnEnum.CreatedAt),
  ];
}