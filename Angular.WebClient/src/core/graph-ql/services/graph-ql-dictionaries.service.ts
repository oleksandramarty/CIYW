import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {
    GET_CATEGORIES_DICTIONARY,
    GET_COUNTRIES_DICTIONARY,
    GET_CURRENCIES_DICTIONARY, GET_FREQUENCIES_DICTIONARY,
    GET_SITE_SETTINGS
} from "../queries/graph-ql-dictionaries.query";
import {
    SiteSettingsResponse,
    VersionedListResponseOfCountryResponse,
    VersionedListResponseOfCurrencyResponse,
    VersionedListResponseOfFrequencyResponse,
    VersionedListResponseOfLocaleResponse, VersionedListResponseOfTreeNodeResponseOfCategoryResponse
} from "../../api-clients/common-module.client";
import {ApolloBase} from "apollo-angular";

@Injectable({
    providedIn: 'root',
})
export class GraphQlDictionariesService {
    constructor(
        private readonly apollo: GraphQlService
    ) {
    }

    get apolloClient(): ApolloBase<any> {
        return this.apollo.dictionaries;
    }
    
    public getSiteSettings(): Observable<ApolloQueryResult<{ dictionaries_site_settings: SiteSettingsResponse | undefined }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_SITE_SETTINGS,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_site_settings: SiteSettingsResponse | undefined }>>;
    }

    public getCountriesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_get_countries_dictionary: VersionedListResponseOfCountryResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_COUNTRIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_get_countries_dictionary: VersionedListResponseOfCountryResponse }>>;
    }

    public getCurrenciesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_get_currencies_dictionary: VersionedListResponseOfCurrencyResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_CURRENCIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_get_currencies_dictionary: VersionedListResponseOfCurrencyResponse }>>;
    }

    public getFrequenciesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_get_frequencies_dictionary: VersionedListResponseOfFrequencyResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_FREQUENCIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_get_frequencies_dictionary: VersionedListResponseOfFrequencyResponse }>>;
    }

    public getCategoriesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_get_categories_dictionary: VersionedListResponseOfTreeNodeResponseOfCategoryResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: GET_CATEGORIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_get_categories_dictionary: VersionedListResponseOfTreeNodeResponseOfCategoryResponse }>>;
    }
}