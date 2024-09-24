import { DataItem, IDataItem } from "./data-item.model";
import { ILocaleResponse, LocaleResponse } from "../../api-clients/localizations-client";
import {
  CategoryResponse,
  CountryResponse,
  CurrencyResponse,
  ICategoryResponse,
  ICountryResponse,
  ICurrencyResponse,
  ISiteSettingsResponse, SiteSettingsResponse
} from "../../api-clients/dictionaries-client";
import {environment} from "../../environments/environment";

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
  private _locales: DataItem[] | undefined;
  private _countries: DataItem[] | undefined;
  private _currencies: DataItem[] | undefined;
  private _categories: DataItem[] | undefined;

  constructor() {}

  get locales(): DataItem[] | undefined {
    return this._locales;
  }

  set locales(value: DataItem[] | undefined) {
    if (this._locales === undefined || value !== undefined) {
      this._locales = value;
    }
  }

  get countries(): DataItem[] | undefined {
    return this._countries;
  }

  set countries(value: DataItem[] | undefined) {
    if (this._countries === undefined || value !== undefined) {
      this._countries = value;
    }
  }

  get currencies(): DataItem[] | undefined {
    return this._currencies;
  }

  set currencies(value: DataItem[] | undefined) {
    if (this._currencies === undefined || value !== undefined) {
      this._currencies = value;
    }
  }

  get categories(): DataItem[] | undefined {
    return this._categories;
  }

  set categories(value: DataItem[] | undefined) {
    if (this._categories === undefined || value !== undefined) {
      this._categories = value;
    }
  }
}

export interface IDictionary {
  countries: ICountryResponse[] | undefined;
  currencies: ICurrencyResponse[] | undefined;
  categories: ICategoryResponse[] | undefined;
  locales: ILocaleResponse[] | undefined;

  dataItems: IDictionaryDataItems | undefined;

  siteSettings: ISiteSettingsResponse | undefined;

  buildVersion: string | undefined;
}

export class Dictionary implements IDictionary {
  private _countries: CountryResponse[] | undefined;
  private _currencies: CurrencyResponse[] | undefined;
  private _categories: CategoryResponse[] | undefined;
  private _locales: LocaleResponse[] | undefined;

  private _dataItems: DictionaryDataItems | undefined;
  private _siteSettings: SiteSettingsResponse | undefined;
  private _buildVersion: string | undefined;

  constructor() {}

  get countries(): CountryResponse[] | undefined {
    return this._countries;
  }

  set countries(value: CountryResponse[] | undefined) {
    if (this._countries === undefined || value !== undefined) {
      this._countries = value;
    }
  }

  get currencies(): CurrencyResponse[] | undefined {
    return this._currencies;
  }

  set currencies(value: CurrencyResponse[] | undefined) {
    if (this._currencies === undefined || value !== undefined) {
      this._currencies = value;
    }
  }

  get categories(): CategoryResponse[] | undefined {
    return this._categories;
  }

  set categories(value: CategoryResponse[] | undefined) {
    if (this._categories === undefined || value !== undefined) {
      this._categories = value;
    }
  }

  get locales(): LocaleResponse[] | undefined {
    return this._locales;
  }

  set locales(value: LocaleResponse[] | undefined) {
    if (this._locales === undefined || value !== undefined) {
      this._locales = value;
    }
  }

  get dataItems(): DictionaryDataItems | undefined {
    return this._dataItems;
  }

  set dataItems(value: DictionaryDataItems | undefined) {
    if (this._dataItems === undefined || value !== undefined) {
      this._dataItems = value;
    }
  }

  get siteSettings(): SiteSettingsResponse | undefined {
    return this._siteSettings;
  }

  set siteSettings(value: SiteSettingsResponse | undefined) {
    if (this._siteSettings === undefined || value !== undefined) {
      this._siteSettings = value;
    }
  }

  get buildVersion(): string | undefined {
    return this._buildVersion;
  }

  set buildVersion(value: string | undefined) {
    if (this._buildVersion === undefined || value !== undefined) {
      this._buildVersion = value;
    }
  }

  public updateDateItems(): void {
    if (this.dataItems === undefined) {
      this.dataItems = new DictionaryDataItems();
    }

    this.dataItems.countries = this.countries?.map(country => new DataItem(String(country.id), country.titleEn, country.title, country.isActive, false));
    this.dataItems.currencies = this.currencies?.map(currency => new DataItem(String(currency.id), currency.titleEn, `${currency.code} - ${currency.title}`, currency.isActive, false));
    this.dataItems.categories = this.categories?.map(category => new DataItem(String(category.id), category.title, '', category.isActive, false));
    this.dataItems.locales = this.locales?.map(locale => new DataItem(String(locale.id), locale.title, locale.titleEn, locale.isActive, false));
  }

  public isLoaded(): boolean {
    return !!(
      this.countries &&
      this.currencies &&
      this.categories &&
      this.locales &&
      this.siteSettings &&
      this.buildVersion &&
      this.buildVersion === environment.buildVersion &&
      this.dataItems &&
      this.dataItems.countries &&
      this.dataItems.currencies &&
      this.dataItems.categories &&
      this.dataItems.locales
    );
  }
}