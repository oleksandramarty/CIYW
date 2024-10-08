//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.0.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

export const API_BASE_URL_CommonModule = new InjectionToken<string>('API_BASE_URL_CommonModule');


export class SiteSettingsResponse implements ISiteSettingsResponse {
    locale!: string;
    version!: CacheVersionResponse;

    constructor(data?: ISiteSettingsResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.version = new CacheVersionResponse();
        }
    }

    init(_data?: any) {
        if (_data) {
            this.locale = _data["locale"];
            this.version = _data["version"] ? CacheVersionResponse.fromJS(_data["version"]) : new CacheVersionResponse();
        }
    }

    static fromJS(data: any): SiteSettingsResponse {
        data = typeof data === 'object' ? data : {};
        let result = new SiteSettingsResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["locale"] = this.locale;
        data["version"] = this.version ? this.version.toJSON() : <any>undefined;
        return data;
    }
}

export interface ISiteSettingsResponse {
    locale: string;
    version: CacheVersionResponse;
}

export class CacheVersionResponse implements ICacheVersionResponse {
    localizationPublic!: string;
    localization!: string;
    category!: string;
    currency!: string;
    country!: string;
    locale!: string;
    frequency!: string;

    constructor(data?: ICacheVersionResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.localizationPublic = _data["localizationPublic"];
            this.localization = _data["localization"];
            this.category = _data["category"];
            this.currency = _data["currency"];
            this.country = _data["country"];
            this.locale = _data["locale"];
            this.frequency = _data["frequency"];
        }
    }

    static fromJS(data: any): CacheVersionResponse {
        data = typeof data === 'object' ? data : {};
        let result = new CacheVersionResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["localizationPublic"] = this.localizationPublic;
        data["localization"] = this.localization;
        data["category"] = this.category;
        data["currency"] = this.currency;
        data["country"] = this.country;
        data["locale"] = this.locale;
        data["frequency"] = this.frequency;
        return data;
    }
}

export interface ICacheVersionResponse {
    localizationPublic: string;
    localization: string;
    category: string;
    currency: string;
    country: string;
    locale: string;
    frequency: string;
}

export class JwtTokenResponse implements IJwtTokenResponse {
    token!: string;

    constructor(data?: IJwtTokenResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.token = _data["token"];
        }
    }

    static fromJS(data: any): JwtTokenResponse {
        data = typeof data === 'object' ? data : {};
        let result = new JwtTokenResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["token"] = this.token;
        return data;
    }
}

export interface IJwtTokenResponse {
    token: string;
}

export class BaseIdEntityOfGuid implements IBaseIdEntityOfGuid {
    id!: string;

    constructor(data?: IBaseIdEntityOfGuid) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
        }
    }

    static fromJS(data: any): BaseIdEntityOfGuid {
        data = typeof data === 'object' ? data : {};
        let result = new BaseIdEntityOfGuid();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        return data;
    }
}

export interface IBaseIdEntityOfGuid {
    id: string;
}

export class BaseDateTimeEntityOfGuid extends BaseIdEntityOfGuid implements IBaseDateTimeEntityOfGuid {
    created!: Date;
    modified?: Date | undefined;

    constructor(data?: IBaseDateTimeEntityOfGuid) {
        super(data);
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.created = _data["created"] ? new Date(_data["created"].toString()) : <any>undefined;
            this.modified = _data["modified"] ? new Date(_data["modified"].toString()) : <any>undefined;
        }
    }

    static override fromJS(data: any): BaseDateTimeEntityOfGuid {
        data = typeof data === 'object' ? data : {};
        let result = new BaseDateTimeEntityOfGuid();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["created"] = this.created ? this.created.toISOString() : <any>undefined;
        data["modified"] = this.modified ? this.modified.toISOString() : <any>undefined;
        super.toJSON(data);
        return data;
    }
}

export interface IBaseDateTimeEntityOfGuid extends IBaseIdEntityOfGuid {
    created: Date;
    modified?: Date | undefined;
}

export class UserResponse extends BaseDateTimeEntityOfGuid implements IUserResponse {
    login!: string;
    loginNormalized!: string;
    email!: string;
    emailNormalized!: string;
    passwordHash!: string;
    salt!: string;
    isActive!: boolean;
    isTemporaryPassword!: boolean;
    authType!: UserAuthMethodEnum;
    lastForgotPassword?: Date | undefined;
    lastForgotPasswordRequest?: Date | undefined;
    roles!: RoleResponse[];
    userSetting!: UserSettingResponse;
    version!: string;

    constructor(data?: IUserResponse) {
        super(data);
        if (!data) {
            this.roles = [];
            this.userSetting = new UserSettingResponse();
        }
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.login = _data["login"];
            this.loginNormalized = _data["loginNormalized"];
            this.email = _data["email"];
            this.emailNormalized = _data["emailNormalized"];
            this.passwordHash = _data["passwordHash"];
            this.salt = _data["salt"];
            this.isActive = _data["isActive"];
            this.isTemporaryPassword = _data["isTemporaryPassword"];
            this.authType = _data["authType"];
            this.lastForgotPassword = _data["lastForgotPassword"] ? new Date(_data["lastForgotPassword"].toString()) : <any>undefined;
            this.lastForgotPasswordRequest = _data["lastForgotPasswordRequest"] ? new Date(_data["lastForgotPasswordRequest"].toString()) : <any>undefined;
            if (Array.isArray(_data["roles"])) {
                this.roles = [] as any;
                for (let item of _data["roles"])
                    this.roles!.push(RoleResponse.fromJS(item));
            }
            this.userSetting = _data["userSetting"] ? UserSettingResponse.fromJS(_data["userSetting"]) : new UserSettingResponse();
            this.version = _data["version"];
        }
    }

    static override fromJS(data: any): UserResponse {
        data = typeof data === 'object' ? data : {};
        let result = new UserResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["login"] = this.login;
        data["loginNormalized"] = this.loginNormalized;
        data["email"] = this.email;
        data["emailNormalized"] = this.emailNormalized;
        data["passwordHash"] = this.passwordHash;
        data["salt"] = this.salt;
        data["isActive"] = this.isActive;
        data["isTemporaryPassword"] = this.isTemporaryPassword;
        data["authType"] = this.authType;
        data["lastForgotPassword"] = this.lastForgotPassword ? this.lastForgotPassword.toISOString() : <any>undefined;
        data["lastForgotPasswordRequest"] = this.lastForgotPasswordRequest ? this.lastForgotPasswordRequest.toISOString() : <any>undefined;
        if (Array.isArray(this.roles)) {
            data["roles"] = [];
            for (let item of this.roles)
                data["roles"].push(item.toJSON());
        }
        data["userSetting"] = this.userSetting ? this.userSetting.toJSON() : <any>undefined;
        data["version"] = this.version;
        super.toJSON(data);
        return data;
    }
}

export interface IUserResponse extends IBaseDateTimeEntityOfGuid {
    login: string;
    loginNormalized: string;
    email: string;
    emailNormalized: string;
    passwordHash: string;
    salt: string;
    isActive: boolean;
    isTemporaryPassword: boolean;
    authType: UserAuthMethodEnum;
    lastForgotPassword?: Date | undefined;
    lastForgotPasswordRequest?: Date | undefined;
    roles: RoleResponse[];
    userSetting: UserSettingResponse;
    version: string;
}

export enum UserAuthMethodEnum {
    Base = 1,
    Google = 2,
}

export class BaseIdEntityOfInteger implements IBaseIdEntityOfInteger {
    id!: number;

    constructor(data?: IBaseIdEntityOfInteger) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.id = _data["id"];
        }
    }

    static fromJS(data: any): BaseIdEntityOfInteger {
        data = typeof data === 'object' ? data : {};
        let result = new BaseIdEntityOfInteger();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["id"] = this.id;
        return data;
    }
}

export interface IBaseIdEntityOfInteger {
    id: number;
}

export class RoleResponse extends BaseIdEntityOfInteger implements IRoleResponse {
    title!: string;
    userRole!: UserRoleEnum;

    constructor(data?: IRoleResponse) {
        super(data);
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.title = _data["title"];
            this.userRole = _data["userRole"];
        }
    }

    static override fromJS(data: any): RoleResponse {
        data = typeof data === 'object' ? data : {};
        let result = new RoleResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["title"] = this.title;
        data["userRole"] = this.userRole;
        super.toJSON(data);
        return data;
    }
}

export interface IRoleResponse extends IBaseIdEntityOfInteger {
    title: string;
    userRole: UserRoleEnum;
}

export enum UserRoleEnum {
    User = 1,
    TechnicalSupport = 2,
    Admin = 3,
    SuperAdmin = 4,
}

export class UserSettingResponse extends BaseIdEntityOfGuid implements IUserSettingResponse {
    defaultLocale!: string;
    timeZone!: number;
    currencyId?: number | undefined;
    countryId?: number | undefined;
    defaultUserProject!: string;
    userId!: string;
    version!: string;

    constructor(data?: IUserSettingResponse) {
        super(data);
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.defaultLocale = _data["defaultLocale"];
            this.timeZone = _data["timeZone"];
            this.currencyId = _data["currencyId"];
            this.countryId = _data["countryId"];
            this.defaultUserProject = _data["defaultUserProject"];
            this.userId = _data["userId"];
            this.version = _data["version"];
        }
    }

    static override fromJS(data: any): UserSettingResponse {
        data = typeof data === 'object' ? data : {};
        let result = new UserSettingResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["defaultLocale"] = this.defaultLocale;
        data["timeZone"] = this.timeZone;
        data["currencyId"] = this.currencyId;
        data["countryId"] = this.countryId;
        data["defaultUserProject"] = this.defaultUserProject;
        data["userId"] = this.userId;
        data["version"] = this.version;
        super.toJSON(data);
        return data;
    }
}

export interface IUserSettingResponse extends IBaseIdEntityOfGuid {
    defaultLocale: string;
    timeZone: number;
    currencyId?: number | undefined;
    countryId?: number | undefined;
    defaultUserProject: string;
    userId: string;
    version: string;
}

export class ListWithIncludeResponseOfExpenseResponse implements IListWithIncludeResponseOfExpenseResponse {
    entities!: ExpenseResponse[];
    paginator?: PaginatorEntity | undefined;
    totalCount!: number;

    constructor(data?: IListWithIncludeResponseOfExpenseResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.entities = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            if (Array.isArray(_data["entities"])) {
                this.entities = [] as any;
                for (let item of _data["entities"])
                    this.entities!.push(ExpenseResponse.fromJS(item));
            }
            this.paginator = _data["paginator"] ? PaginatorEntity.fromJS(_data["paginator"]) : <any>undefined;
            this.totalCount = _data["totalCount"];
        }
    }

    static fromJS(data: any): ListWithIncludeResponseOfExpenseResponse {
        data = typeof data === 'object' ? data : {};
        let result = new ListWithIncludeResponseOfExpenseResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (Array.isArray(this.entities)) {
            data["entities"] = [];
            for (let item of this.entities)
                data["entities"].push(item.toJSON());
        }
        data["paginator"] = this.paginator ? this.paginator.toJSON() : <any>undefined;
        data["totalCount"] = this.totalCount;
        return data;
    }
}

export interface IListWithIncludeResponseOfExpenseResponse {
    entities: ExpenseResponse[];
    paginator?: PaginatorEntity | undefined;
    totalCount: number;
}

export class ExpenseResponse extends BaseDateTimeEntityOfGuid implements IExpenseResponse {
    title!: string;
    description?: string | undefined;
    amount!: number;
    balanceBefore!: number;
    balanceAfter!: number;
    balanceId!: string;
    date!: Date;
    categoryId!: number;
    userProjectId!: string;
    createdUserId!: string;
    version!: string;

    constructor(data?: IExpenseResponse) {
        super(data);
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.title = _data["title"];
            this.description = _data["description"];
            this.amount = _data["amount"];
            this.balanceBefore = _data["balanceBefore"];
            this.balanceAfter = _data["balanceAfter"];
            this.balanceId = _data["balanceId"];
            this.date = _data["date"] ? new Date(_data["date"].toString()) : <any>undefined;
            this.categoryId = _data["categoryId"];
            this.userProjectId = _data["userProjectId"];
            this.createdUserId = _data["createdUserId"];
            this.version = _data["version"];
        }
    }

    static override fromJS(data: any): ExpenseResponse {
        data = typeof data === 'object' ? data : {};
        let result = new ExpenseResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["title"] = this.title;
        data["description"] = this.description;
        data["amount"] = this.amount;
        data["balanceBefore"] = this.balanceBefore;
        data["balanceAfter"] = this.balanceAfter;
        data["balanceId"] = this.balanceId;
        data["date"] = this.date ? this.date.toISOString() : <any>undefined;
        data["categoryId"] = this.categoryId;
        data["userProjectId"] = this.userProjectId;
        data["createdUserId"] = this.createdUserId;
        data["version"] = this.version;
        super.toJSON(data);
        return data;
    }
}

export interface IExpenseResponse extends IBaseDateTimeEntityOfGuid {
    title: string;
    description?: string | undefined;
    amount: number;
    balanceBefore: number;
    balanceAfter: number;
    balanceId: string;
    date: Date;
    categoryId: number;
    userProjectId: string;
    createdUserId: string;
    version: string;
}

export class PaginatorEntity implements IPaginatorEntity {
    pageNumber!: number;
    pageSize!: number;
    isFull!: boolean;

    constructor(data?: IPaginatorEntity) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.pageNumber = _data["pageNumber"];
            this.pageSize = _data["pageSize"];
            this.isFull = _data["isFull"];
        }
    }

    static fromJS(data: any): PaginatorEntity {
        data = typeof data === 'object' ? data : {};
        let result = new PaginatorEntity();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["pageNumber"] = this.pageNumber;
        data["pageSize"] = this.pageSize;
        data["isFull"] = this.isFull;
        return data;
    }
}

export interface IPaginatorEntity {
    pageNumber: number;
    pageSize: number;
    isFull: boolean;
}

export class ListWithIncludeResponseOfPlannedExpenseResponse implements IListWithIncludeResponseOfPlannedExpenseResponse {
    entities!: PlannedExpenseResponse[];
    paginator?: PaginatorEntity | undefined;
    totalCount!: number;

    constructor(data?: IListWithIncludeResponseOfPlannedExpenseResponse) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
        if (!data) {
            this.entities = [];
        }
    }

    init(_data?: any) {
        if (_data) {
            if (Array.isArray(_data["entities"])) {
                this.entities = [] as any;
                for (let item of _data["entities"])
                    this.entities!.push(PlannedExpenseResponse.fromJS(item));
            }
            this.paginator = _data["paginator"] ? PaginatorEntity.fromJS(_data["paginator"]) : <any>undefined;
            this.totalCount = _data["totalCount"];
        }
    }

    static fromJS(data: any): ListWithIncludeResponseOfPlannedExpenseResponse {
        data = typeof data === 'object' ? data : {};
        let result = new ListWithIncludeResponseOfPlannedExpenseResponse();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        if (Array.isArray(this.entities)) {
            data["entities"] = [];
            for (let item of this.entities)
                data["entities"].push(item.toJSON());
        }
        data["paginator"] = this.paginator ? this.paginator.toJSON() : <any>undefined;
        data["totalCount"] = this.totalCount;
        return data;
    }
}

export interface IListWithIncludeResponseOfPlannedExpenseResponse {
    entities: PlannedExpenseResponse[];
    paginator?: PaginatorEntity | undefined;
    totalCount: number;
}

export class PlannedExpenseResponse extends BaseDateTimeEntityOfGuid implements IPlannedExpenseResponse {
    title!: string;
    description?: string | undefined;
    amount!: number;
    categoryId!: number;
    balanceId!: string;
    startDate!: Date;
    endDate?: Date | undefined;
    userId!: string;
    userProjectId!: string;
    frequencyId!: number;
    isActive!: boolean;
    version!: string;

    constructor(data?: IPlannedExpenseResponse) {
        super(data);
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.title = _data["title"];
            this.description = _data["description"];
            this.amount = _data["amount"];
            this.categoryId = _data["categoryId"];
            this.balanceId = _data["balanceId"];
            this.startDate = _data["startDate"] ? new Date(_data["startDate"].toString()) : <any>undefined;
            this.endDate = _data["endDate"] ? new Date(_data["endDate"].toString()) : <any>undefined;
            this.userId = _data["userId"];
            this.userProjectId = _data["userProjectId"];
            this.frequencyId = _data["frequencyId"];
            this.isActive = _data["isActive"];
            this.version = _data["version"];
        }
    }

    static override fromJS(data: any): PlannedExpenseResponse {
        data = typeof data === 'object' ? data : {};
        let result = new PlannedExpenseResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["title"] = this.title;
        data["description"] = this.description;
        data["amount"] = this.amount;
        data["categoryId"] = this.categoryId;
        data["balanceId"] = this.balanceId;
        data["startDate"] = this.startDate ? this.startDate.toISOString() : <any>undefined;
        data["endDate"] = this.endDate ? this.endDate.toISOString() : <any>undefined;
        data["userId"] = this.userId;
        data["userProjectId"] = this.userProjectId;
        data["frequencyId"] = this.frequencyId;
        data["isActive"] = this.isActive;
        data["version"] = this.version;
        super.toJSON(data);
        return data;
    }
}

export interface IPlannedExpenseResponse extends IBaseDateTimeEntityOfGuid {
    title: string;
    description?: string | undefined;
    amount: number;
    categoryId: number;
    balanceId: string;
    startDate: Date;
    endDate?: Date | undefined;
    userId: string;
    userProjectId: string;
    frequencyId: number;
    isActive: boolean;
    version: string;
}

export class BaseFilterRequest implements IBaseFilterRequest {
    paginator?: PaginatorEntity | undefined;
    sort?: BaseSortableRequest | undefined;
    dateRange?: BaseDateRangeFilterRequest | undefined;
    amountRange?: BaseAmountRangeFilterRequest | undefined;
    query?: string | undefined;

    constructor(data?: IBaseFilterRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.paginator = _data["paginator"] ? PaginatorEntity.fromJS(_data["paginator"]) : <any>undefined;
            this.sort = _data["sort"] ? BaseSortableRequest.fromJS(_data["sort"]) : <any>undefined;
            this.dateRange = _data["dateRange"] ? BaseDateRangeFilterRequest.fromJS(_data["dateRange"]) : <any>undefined;
            this.amountRange = _data["amountRange"] ? BaseAmountRangeFilterRequest.fromJS(_data["amountRange"]) : <any>undefined;
            this.query = _data["query"];
        }
    }

    static fromJS(data: any): BaseFilterRequest {
        data = typeof data === 'object' ? data : {};
        let result = new BaseFilterRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["paginator"] = this.paginator ? this.paginator.toJSON() : <any>undefined;
        data["sort"] = this.sort ? this.sort.toJSON() : <any>undefined;
        data["dateRange"] = this.dateRange ? this.dateRange.toJSON() : <any>undefined;
        data["amountRange"] = this.amountRange ? this.amountRange.toJSON() : <any>undefined;
        data["query"] = this.query;
        return data;
    }
}

export interface IBaseFilterRequest {
    paginator?: PaginatorEntity | undefined;
    sort?: BaseSortableRequest | undefined;
    dateRange?: BaseDateRangeFilterRequest | undefined;
    amountRange?: BaseAmountRangeFilterRequest | undefined;
    query?: string | undefined;
}

export class BaseSortableRequest implements IBaseSortableRequest {
    column?: ColumnEnum | undefined;
    direction?: OrderDirectionEnum | undefined;

    constructor(data?: IBaseSortableRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.column = _data["column"];
            this.direction = _data["direction"];
        }
    }

    static fromJS(data: any): BaseSortableRequest {
        data = typeof data === 'object' ? data : {};
        let result = new BaseSortableRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["column"] = this.column;
        data["direction"] = this.direction;
        return data;
    }
}

export interface IBaseSortableRequest {
    column?: ColumnEnum | undefined;
    direction?: OrderDirectionEnum | undefined;
}

export enum ColumnEnum {
    Date = 1,
    Created = 2,
    Modified = 3,
    Title = 4,
    Description = 5,
    Amount = 6,
}

export enum OrderDirectionEnum {
    Desc = 1,
    Asc = 2,
}

export class BaseDateRangeFilterRequest implements IBaseDateRangeFilterRequest {
    startDate?: Date | undefined;
    endDate?: Date | undefined;

    constructor(data?: IBaseDateRangeFilterRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.startDate = _data["startDate"] ? new Date(_data["startDate"].toString()) : <any>undefined;
            this.endDate = _data["endDate"] ? new Date(_data["endDate"].toString()) : <any>undefined;
        }
    }

    static fromJS(data: any): BaseDateRangeFilterRequest {
        data = typeof data === 'object' ? data : {};
        let result = new BaseDateRangeFilterRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["startDate"] = this.startDate ? this.startDate.toISOString() : <any>undefined;
        data["endDate"] = this.endDate ? this.endDate.toISOString() : <any>undefined;
        return data;
    }
}

export interface IBaseDateRangeFilterRequest {
    startDate?: Date | undefined;
    endDate?: Date | undefined;
}

export class BaseAmountRangeFilterRequest implements IBaseAmountRangeFilterRequest {
    amountFrom?: number | undefined;
    amountTo?: number | undefined;

    constructor(data?: IBaseAmountRangeFilterRequest) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(_data?: any) {
        if (_data) {
            this.amountFrom = _data["amountFrom"];
            this.amountTo = _data["amountTo"];
        }
    }

    static fromJS(data: any): BaseAmountRangeFilterRequest {
        data = typeof data === 'object' ? data : {};
        let result = new BaseAmountRangeFilterRequest();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["amountFrom"] = this.amountFrom;
        data["amountTo"] = this.amountTo;
        return data;
    }
}

export interface IBaseAmountRangeFilterRequest {
    amountFrom?: number | undefined;
    amountTo?: number | undefined;
}

export class UserAllowedProjectResponse extends BaseIdEntityOfGuid implements IUserAllowedProjectResponse {
    userProjectId!: string;
    userProject!: UserProjectResponse;
    userId!: string;
    isReadOnly!: boolean;

    constructor(data?: IUserAllowedProjectResponse) {
        super(data);
        if (!data) {
            this.userProject = new UserProjectResponse();
        }
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.userProjectId = _data["userProjectId"];
            this.userProject = _data["userProject"] ? UserProjectResponse.fromJS(_data["userProject"]) : new UserProjectResponse();
            this.userId = _data["userId"];
            this.isReadOnly = _data["isReadOnly"];
        }
    }

    static override fromJS(data: any): UserAllowedProjectResponse {
        data = typeof data === 'object' ? data : {};
        let result = new UserAllowedProjectResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userProjectId"] = this.userProjectId;
        data["userProject"] = this.userProject ? this.userProject.toJSON() : <any>undefined;
        data["userId"] = this.userId;
        data["isReadOnly"] = this.isReadOnly;
        super.toJSON(data);
        return data;
    }
}

export interface IUserAllowedProjectResponse extends IBaseIdEntityOfGuid {
    userProjectId: string;
    userProject: UserProjectResponse;
    userId: string;
    isReadOnly: boolean;
}

export class UserProjectResponse extends BaseDateTimeEntityOfGuid implements IUserProjectResponse {
    title!: string;
    isActive!: boolean;
    createdUserId!: string;
    balances!: BalanceResponse[];
    version!: string;

    constructor(data?: IUserProjectResponse) {
        super(data);
        if (!data) {
            this.balances = [];
        }
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.title = _data["title"];
            this.isActive = _data["isActive"];
            this.createdUserId = _data["createdUserId"];
            if (Array.isArray(_data["balances"])) {
                this.balances = [] as any;
                for (let item of _data["balances"])
                    this.balances!.push(BalanceResponse.fromJS(item));
            }
            this.version = _data["version"];
        }
    }

    static override fromJS(data: any): UserProjectResponse {
        data = typeof data === 'object' ? data : {};
        let result = new UserProjectResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["title"] = this.title;
        data["isActive"] = this.isActive;
        data["createdUserId"] = this.createdUserId;
        if (Array.isArray(this.balances)) {
            data["balances"] = [];
            for (let item of this.balances)
                data["balances"].push(item.toJSON());
        }
        data["version"] = this.version;
        super.toJSON(data);
        return data;
    }
}

export interface IUserProjectResponse extends IBaseDateTimeEntityOfGuid {
    title: string;
    isActive: boolean;
    createdUserId: string;
    balances: BalanceResponse[];
    version: string;
}

export class BalanceResponse extends BaseDateTimeEntityOfGuid implements IBalanceResponse {
    userId!: string;
    amount!: number;
    currencyId!: number;
    userProjectId!: string;
    version!: string;

    constructor(data?: IBalanceResponse) {
        super(data);
    }

    override init(_data?: any) {
        super.init(_data);
        if (_data) {
            this.userId = _data["userId"];
            this.amount = _data["amount"];
            this.currencyId = _data["currencyId"];
            this.userProjectId = _data["userProjectId"];
            this.version = _data["version"];
        }
    }

    static override fromJS(data: any): BalanceResponse {
        data = typeof data === 'object' ? data : {};
        let result = new BalanceResponse();
        result.init(data);
        return result;
    }

    override toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["userId"] = this.userId;
        data["amount"] = this.amount;
        data["currencyId"] = this.currencyId;
        data["userProjectId"] = this.userProjectId;
        data["version"] = this.version;
        super.toJSON(data);
        return data;
    }
}

export interface IBalanceResponse extends IBaseDateTimeEntityOfGuid {
    userId: string;
    amount: number;
    currencyId: number;
    userProjectId: string;
    version: string;
}

