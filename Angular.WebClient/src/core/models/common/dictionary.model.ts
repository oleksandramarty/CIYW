export class LocalizationData {
  private readonly _data: { [locale: string]: { [key: string]: string } } | undefined;

  constructor(data: { [locale: string]: { [key: string]: string } } | undefined) {
    this._data = data;
  }

  get data(): { [locale: string]: { [key: string]: string } } | undefined {
    return this._data;
    }
}
