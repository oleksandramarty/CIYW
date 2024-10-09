import {
  BalanceResponse, CategoryResponse, CountryResponse, CurrencyResponse,
  FrequencyResponse,
  IBalanceResponse, ICategoryResponse, ICountryResponse,
  ICurrencyResponse,
  IFrequencyResponse, ILocaleResponse, LocaleResponse
} from "../../api-clients/common-module.client";

export interface IDataItem {
  id: string | undefined;
  name: string | undefined;
  description: string | undefined;
  icon: string | undefined;
  color: string | undefined;
  isActive: boolean | undefined;
  isImportant: boolean | undefined;

  children: IDataItem[] | undefined;

  filteredFields?: string[] | undefined;

  originalValue: IFrequencyResponse | IBalanceResponse | ICurrencyResponse | ICategoryResponse | ICountryResponse | ILocaleResponse | undefined;
}

export class DataItem implements IDataItem {
  id: string | undefined;
  name: string | undefined;
  description: string | undefined;
  icon: string | undefined;
  color: string | undefined;
  isActive: boolean | undefined;
  isImportant: boolean | undefined;

  children: DataItem[] | undefined;

  filteredFields?: string[] | undefined;

  originalValue: FrequencyResponse | BalanceResponse | CurrencyResponse | CategoryResponse | CountryResponse | LocaleResponse | undefined;

  constructor(
    originalValue: FrequencyResponse | BalanceResponse | CurrencyResponse | CategoryResponse | CountryResponse | LocaleResponse | undefined,
    id: string | undefined,
    name: string | undefined,
    description: string | undefined,
    icon?: string | undefined,
    color?: string | undefined,
    filteredFields: string[] | undefined = [],
    isActive: boolean | undefined = true,
    isImportant: boolean | undefined = false,
    children: DataItem[] | undefined = []
  ) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.icon = icon;
    this.color = color;
    this.isActive = isActive;
    this.isImportant = isImportant;
    this.children = children;
    this.originalValue = originalValue;
    this.filteredFields = filteredFields;
  }
}
