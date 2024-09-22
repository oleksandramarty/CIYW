export interface ISiteSettings {
  locale: string | undefined;
  apiVersion: string | undefined;
  clientVersion: string | undefined;
}

export class SiteSettings implements ISiteSettings {
  locale: string | undefined;
  apiVersion: string | undefined;
  clientVersion: string | undefined;

  constructor(data: ISiteSettings | undefined) {
    this.locale = data?.locale ?? 'en';
    this.apiVersion = data?.apiVersion;
    this.clientVersion = data?.clientVersion;
  }
}
