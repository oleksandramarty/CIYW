import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {SiteSettingsResponse} from "../../api-clients/common-module.client";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {DICTIONARIES_GET_SITE_SETTINGS} from "../queries/graph-ql-dictionaries.query";

@Injectable({
    providedIn: 'root',
})
export class GraphQlDictionariesService {
    constructor(
        private readonly apollo: GraphQlService
    ) {
    }
    
    public getSiteSettings(): Observable<ApolloQueryResult<{ dictionaries_site_settings: SiteSettingsResponse | undefined }>> {
        return this.apollo.dictionaries
            .watchQuery({
                query: DICTIONARIES_GET_SITE_SETTINGS,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ dictionaries_site_settings: SiteSettingsResponse | undefined }>>;
    }
}