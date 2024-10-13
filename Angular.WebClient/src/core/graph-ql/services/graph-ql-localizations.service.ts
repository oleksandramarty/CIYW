import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {
    LocalizationsResponse,
    VersionedListResponseOfLocaleResponse
} from "../../api-models/common.models";
import {
    GET_LOCALES_DICTIONARY,
    GET_LOCALIZATIONS,
    GET_PUBLIC_LOCALIZATIONS
} from "../queries/graph-ql-localizations.query";
import {ApolloBase} from "apollo-angular";

@Injectable({
    providedIn: 'root',
})
export class GraphQlLocalizationsService {
    constructor(
        private readonly apollo: GraphQlService
    ) {
    }

    get apolloClient(): ApolloBase<any> {
        return this.apollo.localizations;
    }

    public getDictionaryLocales(version: string | undefined): Observable<ApolloQueryResult<{ localizations_get_locales_dictionary: VersionedListResponseOfLocaleResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_LOCALES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ localizations_get_locales_dictionary: VersionedListResponseOfLocaleResponse }>>;
    }

    public getLocalizations(version: string | undefined): Observable<ApolloQueryResult<{ localizations_get_localizations: LocalizationsResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_LOCALIZATIONS,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ localizations_get_localizations: LocalizationsResponse }>>;
    }

    public getPublicLocalizations(version: string | undefined): Observable<ApolloQueryResult<{ localizations_get_public_localizations: LocalizationsResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_PUBLIC_LOCALIZATIONS,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ localizations_get_public_localizations: LocalizationsResponse }>>;
    }
}