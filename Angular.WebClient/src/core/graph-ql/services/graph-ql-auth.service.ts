import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {ApolloQueryResult} from "@apollo/client";
import {GraphQlService} from "../graph-ql.service";
import {AUTH_GATEWAY_CURRENT_USER, AUTH_GATEWAY_SIGN_IN, AUTH_GATEWAY_SIGN_OUT} from "../queries/graph-ql-auth.query";
import {BaseBoolResponse, JwtTokenResponse, UserResponse} from "../../api-models/common.models";
import {ApolloBase} from "apollo-angular";

@Injectable({
    providedIn: 'root',
})
export class GraphQlAuthService {
    constructor(
        private readonly apollo: GraphQlService
    ) {
    }

    get apolloClient(): ApolloBase<any> {
        return this.apollo.authGateway;
    }

    public signIn(login: string, password: string, rememberMe: boolean): Observable<ApolloQueryResult<{ auth_gateway_sign_in: JwtTokenResponse | undefined }>> {
        return this.apolloClient
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

    public signOut(): Observable<ApolloQueryResult<BaseBoolResponse>> {
        return this.apolloClient
            .watchQuery({
                query: AUTH_GATEWAY_SIGN_OUT,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<BaseBoolResponse>>;
    }

    public currentUser(): Observable<ApolloQueryResult<{ auth_gateway_current_user: UserResponse | undefined }>> {
        return this.apolloClient
            .watchQuery({
                query: AUTH_GATEWAY_CURRENT_USER,
                fetchPolicy: 'network-only',
            }).valueChanges as Observable<ApolloQueryResult<{ auth_gateway_current_user: UserResponse | undefined }>>;
    }
}