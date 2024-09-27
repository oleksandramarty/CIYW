import {DataItem, IDataItem} from "./data-item.model";
import {ILocaleResponse, LocaleResponse} from "../../api-clients/localizations-client";
import {environment} from "../../environments/environment";
import {LocalStorageService} from "../../services/local-storage.service";
import {
    ISiteSettingsResponse,
    IVersionedListOfCountryResponse,
    IVersionedListOfCurrencyResponse,
    IVersionedListOfTreeNodeResponseOfCategoryResponse,
    SiteSettingsResponse,
    TreeNodeResponseOfCategoryResponse,
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
    private _locales: DataItem[] | undefined;
    private _countries: DataItem[] | undefined;
    private _currencies: DataItem[] | undefined;
    private _categories: DataItem[] | undefined;

    constructor() {
    }

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
    countries: IVersionedListOfCountryResponse | undefined;
    currencies: IVersionedListOfCurrencyResponse | undefined;
    categories: IVersionedListOfTreeNodeResponseOfCategoryResponse | undefined;
    locales: ILocaleResponse[] | undefined;

    dataItems: IDictionaryDataItems | undefined;

    buildVersion: string | undefined;
}

export class Dictionary implements IDictionary {
    private _countries: VersionedListOfCountryResponse | undefined;
    private _currencies: VersionedListOfCurrencyResponse | undefined;
    private _categories: VersionedListOfTreeNodeResponseOfCategoryResponse | undefined;
    private _locales: LocaleResponse[] | undefined;

    private _dataItems: DictionaryDataItems | undefined;
    private _buildVersion: string | undefined;

    constructor(
        private readonly localStorageService: LocalStorageService
    ) {
    }

    get countries(): VersionedListOfCountryResponse | undefined {
        return this._countries;
    }

    get countriesVersion(): string | undefined {
        return this._countries?.version;
    }

    get countriesCount(): string | undefined {
        return String(this._countries?.items?.length ?? 0);
    }

    set countries(value: VersionedListOfCountryResponse | undefined) {
        if (this._countries === undefined || value !== undefined) {
            this._countries = value;
            this._updateCache();
        }
    }

    get currencies(): VersionedListOfCurrencyResponse | undefined {
        return this._currencies;
    }

    get currenciesVersion(): string | undefined {
        return this._currencies?.version;
    }

    get currenciesCount(): string | undefined {
        return String(this._currencies?.items?.length ?? 0);
    }

    set currencies(value: VersionedListOfCurrencyResponse | undefined) {
        if (this._currencies === undefined || value !== undefined) {
            this._currencies = value;
            this._updateCache();
        }
    }

    get categories(): VersionedListOfTreeNodeResponseOfCategoryResponse | undefined {
        return this._categories;
    }

    get categoriesVersion(): string | undefined {
        return this._categories?.version;
    }

    get categoriesCount(): string | undefined {
        return String(this._categories?.items?.length ?? 0);
    }

    set categories(value: VersionedListOfTreeNodeResponseOfCategoryResponse | undefined) {
        if (this._categories === undefined || value !== undefined) {
            this._categories = value;
            this._updateCache();
        }
    }


    get locales(): LocaleResponse[] | undefined {
        return this._locales;
    }

    set locales(value: LocaleResponse[] | undefined) {
        if (this._locales === undefined || value !== undefined) {
            this._locales = value;
            this._updateCache();
        }
    }

    get dataItems(): DictionaryDataItems | undefined {
        return this._dataItems;
    }

    set dataItems(value: DictionaryDataItems | undefined) {
        if (this._dataItems === undefined || value !== undefined) {
            this._dataItems = value;
            this._updateCache();
        }
    }

    get buildVersion(): string | undefined {
        return this._buildVersion;
    }

    set buildVersion(value: string | undefined) {
        if (this._buildVersion === undefined || value !== undefined) {
            this._buildVersion = value;
            this._updateCache();
        }
    }

    public updateDateItems(): void {
        if (this.dataItems === undefined) {
            this.dataItems = new DictionaryDataItems();
        }

        this.dataItems.countries = this.countries?.items.map(country => new DataItem(String(country.id), country.titleEn, country.title));
        this.dataItems.currencies = this.currencies?.items.map(currency => new DataItem(String(currency.id), currency.titleEn, `${currency.code} - ${currency.title}`));
        this.dataItems.categories = this._mapCategories(this.categories?.items || []);
        this.dataItems.locales = this.locales?.map(locale => new DataItem(String(locale.id), locale.title, locale.titleEn));

        this._updateCache();
    }

    public isLoaded(): boolean {
        return !!(
            this.countries &&
            this.currencies &&
            this.categories &&
            this.locales &&
            this.buildVersion &&
            this.buildVersion === environment.buildVersion &&
            this.dataItems &&
            this.dataItems.countries &&
            this.dataItems.currencies &&
            this.dataItems.categories &&
            this.dataItems.locales
        );
    }

    private _updateCache(): void {
        this.localStorageService.setItem('dictionaries', this);
    }

    private _mapCategories(categories: TreeNodeResponseOfCategoryResponse[]): DataItem[] {
        return categories.map(category => this._mapCategory(category));
    }

    private _mapCategory(category: TreeNodeResponseOfCategoryResponse): DataItem {
        const dataItem: DataItem = {
            id: category.node?.id?.toString(),
            name: category.node?.title,
            description: category.node?.icon,
            isActive: category.node?.isActive,
            isImportant: category.node?.isPositive,
            children: category.node?.children ? this._mapCategories(category.node.children) : []
        };
        return dataItem;
    }
}