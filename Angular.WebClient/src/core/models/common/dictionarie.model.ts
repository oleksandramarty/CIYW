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

export class DictionaryMap<TKey, TValue> {
  private items: Map<TKey, TValue>;

  constructor() {
    this.items = new Map<TKey, TValue>();
  }

  public set(key: TKey, value: TValue): void {
    this.items.set(key, value);
  }

  public get(key: TKey): TValue | undefined {
    return this.items.get(key);
  }

  public has(key: TKey): boolean {
    return this.items.has(key);
  }

  public delete(key: TKey): void {
    this.items.delete(key);
  }

  public clear(): void {
    this.items.clear();
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
}

export class Dictionary implements IDictionary {
    countries: VersionedListOfCountryResponse | undefined;
    currencies: VersionedListOfCurrencyResponse | undefined;
    categories: VersionedListOfTreeNodeResponseOfCategoryResponse | undefined;
    locales: VersionedListOfLocaleResponse | undefined;
}