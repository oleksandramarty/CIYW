import {DataItem, IDataItem} from "./data-item.model";
import {ILocaleResponse, LocaleResponse} from "../../api-clients/localizations-client";

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

export interface IDictionary {
  locales: IDataItem[] | undefined;
  localeResponses: ILocaleResponse[] | undefined;
}

export class Dictionary implements IDictionary{
  locales: DataItem[] | undefined;
  localeResponses: LocaleResponse[] | undefined;

  constructor(localeResponsesData: ILocaleResponse[] | undefined) {
    if (localeResponsesData) {
      this.localeResponses = localeResponsesData.map(data => new LocaleResponse(data));
      this.locales =
          this.localeResponses.map(l => new DataItem(String(l.id), l.titleEn, l.title, true, false)).filter(item => item !== undefined) as DataItem[];
    } else {
      this.localeResponses = undefined;
      this.locales = undefined;
    }
  }
}
