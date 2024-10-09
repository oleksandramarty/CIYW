import {gql} from "@apollo/client";

export const GET_LOCALES_DICTIONARY = gql`
    query GetLocales($version: String) {
        localizations_get_locales_dictionary(version: $version) {
            items {
                id
                isoCode
                title
                titleEn
                titleNormalized
                titleEnNormalized
                isDefault
                isActive
                localeEnum
                culture
            }
            version
        }
    }
`;

export const GET_PUBLIC_LOCALIZATIONS = gql`
    query GetPublicLocalizations($version: String) {
        localizations_get_public_localizations(version: $version) {
            version
            data {
                locale
                items {
                    key
                    value
                }
            }
        }
    }
`;

export const GET_LOCALIZATIONS = gql`
    query GetLocalizations($version: String) {
        localizations_get_localizations(version: $version) {
            version
            data {
                locale
                items {
                    key
                    value
                }
            }
        }
    }
`;