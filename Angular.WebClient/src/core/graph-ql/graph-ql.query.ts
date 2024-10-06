import {gql} from "@apollo/client";

export const DICTIONARIES_GET_SITE_SETTINGS = gql`
    query GetSiteSettings {
        dictionaries_site_settings {
            locale
            version {
                localizationPublic
                localization
                category
                currency
                country
                locale
                frequency
            }
        }
    }
`;

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
            countryId
            currencyId
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
                defaultTimeZone
                defaultCurrency
                defaultUserProject
                userId
                version
            }
            version
        }
    }
`;
