import {DataItem, IDataItem} from "./data-item.model";
import {
    IVersionedListOfLocaleResponse,
    VersionedListOfLocaleResponse
} from "../../api-clients/localizations-client";
import {
    IVersionedListOfCountryResponse,
    IVersionedListOfCurrencyResponse,
    IVersionedListOfTreeNodeResponseOfCategoryResponse,
    VersionedListOfCountryResponse,
    VersionedListOfCurrencyResponse,
    VersionedListOfTreeNodeResponseOfCategoryResponse
} from "../../api-clients/dictionaries-client";

export interface IKeyValue {
    key: string | undefined;
    value: string | undefined;
}

export class KeyValue implements IKeyValue {
    key: string | undefined;
    value: string | undefined;

    constructor(key: string | undefined, value: string | undefined) {
        this.key = key;
        this.value = value;
    }
}

export interface IDictionaryDataItems {
    locales: IDataItem[] | undefined;
    countries: IDataItem[] | undefined;
    currencies: IDataItem[] | undefined;
    categories: IDataItem[] | undefined;
}

export class DictionaryDataItems implements IDictionaryDataItems {
    locales: DataItem[] | undefined;
    countries: DataItem[] | undefined;
    currencies: DataItem[] | undefined;
    categories: DataItem[] | undefined;
}

export interface IDictionary {
    countries: IVersionedListOfCountryResponse | undefined;
    currencies: IVersionedListOfCurrencyResponse | undefined;
    categories: IVersionedListOfTreeNodeResponseOfCategoryResponse | undefined;
    locales: IVersionedListOfLocaleResponse | undefined;

    dataItems: IDictionaryDataItems | undefined;
}

export class Dictionary implements IDictionary {
    countries: VersionedListOfCountryResponse | undefined;
    currencies: VersionedListOfCurrencyResponse | undefined;
    categories: VersionedListOfTreeNodeResponseOfCategoryResponse | undefined;
    locales: VersionedListOfLocaleResponse | undefined;

     dataItems: DictionaryDataItems | undefined;
}