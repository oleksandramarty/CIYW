import {DataItem} from "../common/data-item.model";
import {LocaleEnum} from "../../enums/locale.enum";

export interface ILocaleResponse {
    id: number | undefined;
    isoCode: string | undefined;
    title: string | undefined;
    titleEn: string | undefined;
    titleNormalized: string | undefined;
    titleEnNormalized: string | undefined;
    isDefault: boolean | undefined;
    isActive: boolean | undefined;
    localeEnum: LocaleEnum | undefined;
    culture: string | undefined;
}

export class LocaleResponse implements ILocaleResponse {
    id: number | undefined;
    isoCode: string | undefined;
    title: string | undefined;
    titleEn: string | undefined;
    titleNormalized: string | undefined;
    titleEnNormalized: string | undefined;
    isDefault: boolean | undefined;
    isActive: boolean | undefined;
    localeEnum: LocaleEnum | undefined;
    culture: string | undefined;

    constructor(
        data: ILocaleResponse | undefined
    ) {
        this.id = data?.id ?? undefined;
        this.isoCode = data?.isoCode ?? undefined;
        this.title = data?.title ?? undefined;
        this.titleEn = data?.titleEn ?? undefined;
        this.titleNormalized = data?.titleNormalized ?? undefined;
        this.titleEnNormalized = data?.titleEnNormalized ?? undefined;
        this.isDefault = data?.isDefault ?? undefined;
        this.isActive = data?.isActive ?? undefined;
        this.localeEnum = data?.localeEnum ?? undefined;
        this.culture = data?.culture ?? undefined;
    }

    public toDataItem(): DataItem | undefined {
        if (this.id === undefined || this.title === undefined || this.titleEn === undefined || this.isActive === undefined) {
            return undefined;
        }
        return new DataItem(
            this.id.toString(),
            this.title,
            this.titleEn,
            this.isActive,
            false
        );
    }
}
