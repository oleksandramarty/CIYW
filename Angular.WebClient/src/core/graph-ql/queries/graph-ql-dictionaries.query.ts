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
