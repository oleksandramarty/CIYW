import {ColumnEnum, OrderDirectionEnum} from "../../api-clients/common-module.client";

export interface IBaseGraphQlFilteredModel {
    dateFrom: Date | undefined;
    dateTo: Date | undefined;
    amountFrom: number | undefined;
    amountTo: number | undefined;
    isFull: boolean;
    pageNumber: number;
    pageSize: number;
    column: string;
    direction: string;
    query: string;
}

export class BaseGraphQlFilteredModel {
    dateFrom: Date | undefined;
    dateTo: Date | undefined;
    amountFrom: number | undefined;
    amountTo: number | undefined;
    isFull: boolean;
    pageNumber: number;
    pageSize: number;
    column: ColumnEnum;
    direction: OrderDirectionEnum;
    query: string;

    constructor(
        dateFrom?: Date,
        dateTo?: Date,
        amountFrom?: number,
        amountTo?: number,
        isFull: boolean = false,
        pageNumber: number = 1,
        pageSize: number = 10,
        column: ColumnEnum = ColumnEnum.Created,
        direction: OrderDirectionEnum = OrderDirectionEnum.Desc,
        query: string = ''
    ) {
        this.dateFrom = dateFrom;
        this.dateTo = dateTo;
        this.amountFrom = amountFrom;
        this.amountTo = amountTo;
        this.isFull = isFull;
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.column = column;
        this.direction = direction;
        this.query = query;
    }
}