import {gql} from "@apollo/client";

export const SITE_SETTINGS = gql`
    query SiteSettings {
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

export const COUNTRIES_DICTIONARY = gql`
    query Countries($version: String) {
        dictionaries_countries_dictionary(version: $version) {
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

export const CURRENCIES_DICTIONARY = gql`
    query Currencies($version: String) {
        dictionaries_currencies_dictionary(version: $version) {
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

export const FREQUENCIES_DICTIONARY = gql`
    query Frequencies($version: String) {
        dictionaries_frequencies_dictionary(version: $version) {
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

export const BALANCE_TYPES_DICTIONARY = gql`
    query BalanceTypesDictionary($version: String) {
        dictionaries_balance_types_dictionary(version: $version) {
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

export const CATEGORIES_DICTIONARY = gql`
    query Categories($version: String) {
        dictionaries_categories_dictionary(version: $version) {
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

export const ICON_CATEGORIES_DICTIONARY = gql`
    query IconCategories($version: String) {
        dictionaries_icon_categories_dictionary(version: $version) {
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

export const NON_PUBLIC_DICTIONARIES = gql`
    query NonPublicDictionaries(
        $versionIconCategories: String,
        $versionCategories: String,
        $versionBalanceTypes: String,
        $versionFrequencies: String,
        $versionCurrencies: String,
        $versionCountries: String
    ) {
        dictionaries_icon_categories_dictionary(version: $versionIconCategories) {
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
        dictionaries_categories_dictionary(version: $versionCategories) {
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
        dictionaries_balance_types_dictionary(version: $versionBalanceTypes) {
            items {
                id
                title
                isActive
                type
            }
            version
        }
        dictionaries_frequencies_dictionary(version: $versionFrequencies) {
            items {
                id
                title
                description
                isActive
                type
            }
            version
        }
        dictionaries_currencies_dictionary(version: $versionCurrencies) {
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
        dictionaries_countries_dictionary(version: $versionCountries) {
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