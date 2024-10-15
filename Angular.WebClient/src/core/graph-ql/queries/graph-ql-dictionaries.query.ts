import {gql} from "@apollo/client";

export const GET_SITE_SETTINGS = gql`
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
                balanceType
                iconCategory
            }
        }
    }
`;

export const GET_COUNTRIES_DICTIONARY = gql`
    query GetCountries($version: String) {
        dictionaries_get_countries_dictionary(version: $version) {
            items {
                id
                title
                code
                titleEn
                isActive
            }
            version
        }
    }
`;

export const GET_CURRENCIES_DICTIONARY = gql`
    query GetCurrencies($version: String) {
        dictionaries_get_currencies_dictionary(version: $version) {
            items {
                id
                title
                code
                symbol
                titleEn
                isActive
            }
            version
        }
    }
`;

export const GET_FREQUENCIES_DICTIONARY = gql`
    query GetFrequencies($version: String) {
        dictionaries_get_frequencies_dictionary(version: $version) {
            items {
                id
                title
                description
                isActive
                type
            }
            version
        }
    }
`;

export const GET_BALANCE_TYPES_DICTIONARY = gql`
    query GetBalanceTypesDictionary($version: String) {
        dictionaries_get_balance_types_dictionary(version: $version) {
            items {
                id
                title
                isActive
                type
            }
            version
        }
    }
`;

export const GET_CATEGORIES_DICTIONARY = gql`
    query GetCategories($version: String) {
        dictionaries_get_categories_dictionary(version: $version) {
            items {
                ...CategoryFields
                children {
                    ...CategoryFields
                    children {
                        ...CategoryFields
                        children {
                            ...CategoryFields
                        }
                    }
                }
            }
            version
        }
    }

    fragment CategoryFields on CategoryResponse {
        id
        title
        iconId
        color
        isActive
        isPositive
        parentId
    }
`;

export const GET_ICON_CATEGORIES_DICTIONARY = gql`
    query GetIconCategories($version: String) {
        dictionaries_get_icon_categories_dictionary(version: $version) {
            items {
                id
                title
                isActive
                icons {
                    id
                    title
                    isActive
                    iconCategoryId
                }
            }
            version
        }
    }
`;