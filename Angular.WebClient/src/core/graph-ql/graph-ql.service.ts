import {Injectable} from "@angular/core";
import {Apollo, ApolloBase} from "apollo-angular";
import {apolloEnvironments} from "../helpers/apolo.helper";

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
}