import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {
    BALANCE_TYPES_DICTIONARY,
    CATEGORIES_DICTIONARY,
    COUNTRIES_DICTIONARY,
    CURRENCIES_DICTIONARY,
    FREQUENCIES_DICTIONARY,
    ICON_CATEGORIES_DICTIONARY,
    NON_PUBLIC_DICTIONARIES,
    SITE_SETTINGS
} from "../queries/graph-ql-dictionaries.query";
import {
    SiteSettingsResponse, VersionedListResponseOfBalanceTypeResponse, VersionedListResponseOfCategoryResponse,
    VersionedListResponseOfCountryResponse,
    VersionedListResponseOfCurrencyResponse,
    VersionedListResponseOfFrequencyResponse, VersionedListResponseOfIconCategoryResponse
} from "../../api-models/common.models";
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
    
    public siteSettings(): Observable<ApolloQueryResult<{ dictionaries_site_settings: SiteSettingsResponse | undefined }>> {
        return this.apolloClient
            .watchQuery({
                query: SITE_SETTINGS,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_site_settings: SiteSettingsResponse | undefined }>>;
    }

    public countriesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_countries_dictionary: VersionedListResponseOfCountryResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: COUNTRIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_countries_dictionary: VersionedListResponseOfCountryResponse }>>;
    }

    public currenciesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_currencies_dictionary: VersionedListResponseOfCurrencyResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: CURRENCIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_currencies_dictionary: VersionedListResponseOfCurrencyResponse }>>;
    }

    public frequenciesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_frequencies_dictionary: VersionedListResponseOfFrequencyResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: FREQUENCIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_frequencies_dictionary: VersionedListResponseOfFrequencyResponse }>>;
    }

    public balanceTypesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_balance_types_dictionary: VersionedListResponseOfBalanceTypeResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: BALANCE_TYPES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_balance_types_dictionary: VersionedListResponseOfBalanceTypeResponse }>>;
    }

    public iconCategoriesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_icon_categories_dictionary: VersionedListResponseOfIconCategoryResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: ICON_CATEGORIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_icon_categories_dictionary: VersionedListResponseOfIconCategoryResponse }>>;
    }

    public categoriesDictionary(version: string | undefined): Observable<ApolloQueryResult<{ dictionaries_categories_dictionary: VersionedListResponseOfCategoryResponse }>> {
        return this.apolloClient
            .watchQuery({
                query: CATEGORIES_DICTIONARY,
                variables: {
                    version,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_categories_dictionary: VersionedListResponseOfCategoryResponse }>>;
    }

    public nonPublicDictionaries(
        versionIconCategories: string | undefined,
        versionCategories: string | undefined,
        versionBalanceTypes: string | undefined,
        versionFrequencies: string | undefined,
        versionCurrencies: string | undefined,
        versionCountries: string | undefined,
    ):
        Observable<ApolloQueryResult<{
        dictionaries_icon_categories_dictionary: VersionedListResponseOfIconCategoryResponse
        dictionaries_categories_dictionary: VersionedListResponseOfCategoryResponse,
        dictionaries_balance_types_dictionary: VersionedListResponseOfBalanceTypeResponse,
        dictionaries_frequencies_dictionary: VersionedListResponseOfFrequencyResponse,
        dictionaries_currencies_dictionary: VersionedListResponseOfCurrencyResponse,
        dictionaries_countries_dictionary: VersionedListResponseOfCountryResponse
    }>> {
        return this.apolloClient
            .watchQuery({
                query: NON_PUBLIC_DICTIONARIES,
                variables: {
                    versionIconCategories,
                    versionCategories,
                    versionBalanceTypes,
                    versionFrequencies,
                    versionCurrencies,
                    versionCountries,
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{
            dictionaries_icon_categories_dictionary: VersionedListResponseOfIconCategoryResponse
            dictionaries_categories_dictionary: VersionedListResponseOfCategoryResponse,
            dictionaries_balance_types_dictionary: VersionedListResponseOfBalanceTypeResponse,
            dictionaries_frequencies_dictionary: VersionedListResponseOfFrequencyResponse,
            dictionaries_currencies_dictionary: VersionedListResponseOfCurrencyResponse,
            dictionaries_countries_dictionary: VersionedListResponseOfCountryResponse
        }>>;
    }
}