import {gql} from "@apollo/client";

export const AUTH_GATEWAY_SIGN_IN = gql`
    query SignIn($input: AuthSignInRequestInputType!) {
        auth_gateway_sign_in(input: $input) {
            token
        }
    }
`;

export const AUTH_GATEWAY_CURRENT_USER = gql`
    query GetUserDetails {
        auth_gateway_current_user {
            id
            login
            email
            isActive
            isTemporaryPassword
            authType
            lastForgotPassword
            lastForgotPasswordRequest
            roles {
                id
                title
                userRole
            }
            userSetting {
                id
                defaultLocale
                timeZone
                countryId
                currencyId
                defaultUserProject
                userId
                version
            }
            version
        }
    }
`;
