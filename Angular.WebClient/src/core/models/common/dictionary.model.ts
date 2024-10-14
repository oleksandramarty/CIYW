import {DataItem, IDataItem} from "./data-item.model";
import {
    IVersionedListResponseOfBalanceTypeResponse,
    IVersionedListResponseOfCategoryResponse,
    IVersionedListResponseOfCountryResponse,
    IVersionedListResponseOfCurrencyResponse,
    IVersionedListResponseOfFrequencyResponse,
    IVersionedListResponseOfLocaleResponse,
    VersionedListResponseOfBalanceTypeResponse,
    VersionedListResponseOfCategoryResponse,
    VersionedListResponseOfCountryResponse,
    VersionedListResponseOfCurrencyResponse,
    VersionedListResponseOfFrequencyResponse,
    VersionedListResponseOfLocaleResponse
} from "../../api-models/common.models";

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
    balanceTypes: IDataItem[] | undefined;

    categoriesFlat: IDataItem[] | undefined;
}

export class DictionaryDataItems implements IDictionaryDataItems {
    locales: DataItem[] | undefined;
    countries: DataItem[] | undefined;
    currencies: DataItem[] | undefined;
    categories: DataItem[] | undefined;
    frequencies: DataItem[] | undefined;
    balanceTypes: DataItem[] | undefined;

    categoriesFlat: DataItem[] | undefined;
}

export interface IDictionary {
    countries: IVersionedListResponseOfCountryResponse | undefined;
    currencies: IVersionedListResponseOfCurrencyResponse | undefined;
    categories: IVersionedListResponseOfCategoryResponse | undefined;
    locales: IVersionedListResponseOfLocaleResponse | undefined;
    frequencies: IVersionedListResponseOfFrequencyResponse | undefined;
    balanceTypes: IVersionedListResponseOfBalanceTypeResponse | undefined;
}

export class Dictionary implements IDictionary {
    countries: VersionedListResponseOfCountryResponse | undefined;
    currencies: VersionedListResponseOfCurrencyResponse | undefined;
    categories: VersionedListResponseOfCategoryResponse | undefined;
    locales: VersionedListResponseOfLocaleResponse | undefined;
    frequencies: VersionedListResponseOfFrequencyResponse | undefined;
    balanceTypes: VersionedListResponseOfBalanceTypeResponse | undefined;
}