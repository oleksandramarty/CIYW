import {Injectable} from "@angular/core";
import {Apollo, ApolloBase} from "apollo-angular";
import {apolloEnvironments} from "../helpers/apolo.helper";
import {ColumnEnum, OrderDirectionEnum} from "../api-clients/common-module.client";

@Injectable({
    providedIn: 'root',
})
export class GraphQlService {
    constructor(
        private readonly _apollo: Apollo
    ) {
    }

    get dictionaries(): ApolloBase<any> {
        return this._apollo.use(apolloEnvironments.dictionaries);
    }

    get authGateway(): ApolloBase<any> {
        return this._apollo.use(apolloEnvironments.authGateway);
    }

    get localizations(): ApolloBase<any> {
        return this._apollo.use(apolloEnvironments.localizations);
    }

    get expenses(): ApolloBase<any> {
        return this._apollo.use(apolloEnvironments.expenses);
    }

    get auditTrail(): ApolloBase<any> {
        return this._apollo.use(apolloEnvironments.auditTrail);
    }

    public handleBaseFilter(
        dateFrom: Date | undefined,
        dateTo: Date | undefined,
        amountFrom: number | undefined,
        amountTo: number | undefined,
        isFull: boolean = false,
        pageNumber: number = 1,
        pageSize: number = 10,
        column: ColumnEnum = ColumnEnum.Date,
        direction: OrderDirectionEnum = OrderDirectionEnum.Desc,
        query: string = ''
    ): any | undefined {
        return {
            dateFrom,
            dateTo,
            amountFrom,
            amountTo,
            isFull,
            pageNumber,
            pageSize,
            column: column.toString(),
            direction: direction.toString(),
            query
        }
    }
}