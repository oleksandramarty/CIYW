import {DataItem, IDataItem} from "./data-item.model";
import {
    IVersionedListOfLocaleResponse,
    VersionedListOfLocaleResponse
} from "../../api-clients/localizations-client";
import {
    IVersionedListOfCountryResponse,
    IVersionedListOfCurrencyResponse, IVersionedListOfFrequencyResponse,
    IVersionedListOfTreeNodeResponseOfCategoryResponse,
    VersionedListOfCountryResponse,
    VersionedListOfCurrencyResponse,
    VersionedListOfTreeNodeResponseOfCategoryResponse
} from "../../api-clients/dictionaries-client";

export class DictionaryMap<TKey, TValue> {
  private _items: Map<TKey, TValue>;

  get items(): Map<TKey, TValue> {
    return this._items;
  }

  constructor() {
    this._items = new Map<TKey, TValue>();
  }

  public set(key: TKey, value: TValue): void {
    this._items.set(key, value);
  }

  public get(key: TKey): TValue | undefined {
    return this._items.get(key);
  }

  public has(key: TKey): boolean {
    return this._items.has(key);
  }

  public delete(key: TKey): void {
    this._items.delete(key);
  }

  public clear(): void {
    this._items.clear();
  }
}

export interface IDictionaryDataItems {
    locales: IDataItem[] | undefined;
    countries: IDataItem[] | undefined;
    currencies: IDataItem[] | undefined;
    categories: IDataItem[] | undefined;
    frequencies: IDataItem[] | undefined;

    categoriesFlat: IDataItem[] | undefined;
}

export class DictionaryDataItems implements IDictionaryDataItems {
    locales: DataItem[] | undefined;
    countries: DataItem[] | undefined;
    currencies: DataItem[] | undefined;
    categories: DataItem[] | undefined;
    frequencies: DataItem[] | undefined;

    categoriesFlat: DataItem[] | undefined;
}

export interface IDictionary {
    countries: IVersionedListOfCountryResponse | undefined;
    currencies: IVersionedListOfCurrencyResponse | undefined;
    categories: IVersionedListOfTreeNodeResponseOfCategoryResponse | undefined;
    locales: IVersionedListOfLocaleResponse | undefined;
    frequencies: IVersionedListOfFrequencyResponse | undefined;
}

export class Dictionary implements IDictionary {
    countries: VersionedListOfCountryResponse | undefined;
    currencies: VersionedListOfCurrencyResponse | undefined;
    categories: VersionedListOfTreeNodeResponseOfCategoryResponse | undefined;
    locales: VersionedListOfLocaleResponse | undefined;
    frequencies: IVersionedListOfFrequencyResponse | undefined;
}