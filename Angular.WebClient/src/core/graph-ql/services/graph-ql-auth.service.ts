import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {JwtTokenResponse, SiteSettingsResponse, UserResponse} from "../../api-clients/common-module.client";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {AUTH_GATEWAY_CURRENT_USER, AUTH_GATEWAY_SIGN_IN} from "../queries/graph-ql-auth.query";

@Injectable({
    providedIn: 'root',
})
export class GraphQlAuthService {
    constructor(
        private readonly apollo: GraphQlService
    ) {
    }

    public signIn(login: string, password: string, rememberMe: boolean): Observable<ApolloQueryResult<{ auth_gateway_sign_in: JwtTokenResponse | undefined }>> {
        return this.apollo.authGateway
            .watchQuery({
                query: AUTH_GATEWAY_SIGN_IN,
                variables: {
                    input: {
                        login,
                        password,
                        rememberMe
                    }
                },
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ auth_gateway_sign_in: JwtTokenResponse | undefined }>>;
    }

    public getCurrentUser(): Observable<ApolloQueryResult<{ auth_gateway_current_user: UserResponse | undefined }>> {
        return this.apollo.authGateway
            .watchQuery({
                query: AUTH_GATEWAY_CURRENT_USER,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ auth_gateway_current_user: UserResponse | undefined }>>;
    }
}