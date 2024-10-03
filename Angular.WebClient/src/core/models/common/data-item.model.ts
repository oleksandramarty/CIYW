import {CategoryResponse, CountryResponse, CurrencyResponse} from "../../api-clients/dictionaries-client";
import {LocaleResponse} from "../../api-clients/localizations-client";

export interface IDataItem {
  id: string | undefined;
  name: string | undefined;
  description: string | undefined;
  isActive: boolean | undefined;
  isImportant: boolean | undefined;

  children: IDataItem[] | undefined;

  filteredFields?: string[] | undefined;

  originalValue: CurrencyResponse | CategoryResponse | CountryResponse | LocaleResponse | undefined;
}

export class DataItem implements IDataItem {
  id: string | undefined;
  name: string | undefined;
  description: string | undefined;
  isActive: boolean | undefined;
  isImportant: boolean | undefined;

  children: DataItem[] | undefined;

  filteredFields?: string[] | undefined;

  originalValue: CurrencyResponse | CategoryResponse | CountryResponse | LocaleResponse | undefined;

  constructor(
    originalValue: CurrencyResponse | CategoryResponse | CountryResponse | LocaleResponse | undefined,
    id: string | undefined,
    name: string | undefined,
    description: string | undefined,
    filteredFields: string[] | undefined = [],
    isActive: boolean | undefined = true,
    isImportant: boolean | undefined = false,
    children: DataItem[] | undefined = []
  ) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.isActive = isActive;
    this.isImportant = isImportant;
    this.children = children;
    this.originalValue = originalValue;
    this.filteredFields = filteredFields;
  }
}
