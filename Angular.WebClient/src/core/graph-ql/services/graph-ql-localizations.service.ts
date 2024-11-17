import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {
    LocalizationsResponse,
    VersionedListResponseOfLocaleResponse
} from "../../api-models/common.models";
import {
    LOCALES_DICTIONARY,
    LOCALIZATIONS,
    PUBLIC_LOCALIZATIONS
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

    public dictionaryLocales(version: string | undefined): Observable<ApolloQueryResult<{ localizations_locales_dictionary: VersionedListResponseOfLocaleResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: LOCALES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ localizations_locales_dictionary: VersionedListResponseOfLocaleResponse }>>;
    }

    public localizations(version: string | undefined): Observable<ApolloQueryResult<{ localizations_localizations: LocalizationsResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: LOCALIZATIONS,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ localizations_localizations: LocalizationsResponse }>>;
    }

    public publicLocalizations(version: string | undefined): Observable<ApolloQueryResult<{ localizations_public_localizations: LocalizationsResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: PUBLIC_LOCALIZATIONS,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ localizations_public_localizations: LocalizationsResponse }>>;
    }
}