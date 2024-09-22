import {UserRoleEnum} from "../../enums/user.enum";

export interface IRoleResponse {
    id: number | undefined;
    title: string | undefined;
    userRole: UserRoleEnum | undefined;
}

export class RoleResponse implements IRoleResponse {
    id: number | undefined;
    title: string | undefined;
    userRole: UserRoleEnum | undefined;

    constructor(id: number | undefined, title: string | undefined, userRole: UserRoleEnum | undefined) {
        this.id = id;
        this.title = title;
        this.userRole = userRole;
    }
}

export interface IUserRoleResponse {
    id: string | undefined;
    userId: string | undefined;
    roleId: number | undefined;
    role: RoleResponse | undefined;
}

export class UserRoleResponse implements IUserRoleResponse {
    id: string | undefined;
    userId: string | undefined;
    roleId: number | undefined;
    role: RoleResponse | undefined;

    constructor(id: string | undefined, userId: string | undefined, roleId: number | undefined, role: RoleResponse | undefined) {
        this.id = id;
        this.userId = userId;
        this.roleId = roleId;
        this.role = role;
    }
}

export interface IUserResponse {
    id: string | undefined;
    created: Date | undefined;
    updated: Date | undefined;
    googleId: string | undefined;
    login: string | undefined;
    loginNormalized: string | undefined;
    email: string | undefined;
    emailNormalized: string | undefined;
    passwordHash: string | undefined;
    salt: string | undefined;
    isActive: boolean | undefined;
    isTemporaryPassword: boolean | undefined;
    roles?: UserRoleResponse[] | undefined;
}

export class UserResponse implements IUserResponse {
    id: string | undefined;
    created: Date | undefined;
    updated: Date | undefined;
    googleId: string | undefined;
    login: string | undefined;
    loginNormalized: string | undefined;
    email: string | undefined;
    emailNormalized: string | undefined;
    passwordHash: string | undefined;
    salt: string | undefined;
    isActive: boolean | undefined;
    isTemporaryPassword: boolean | undefined;
    roles?: UserRoleResponse[] | undefined;

    constructor(
        id: string | undefined,
        created: Date | undefined,
        updated: Date | undefined,
        googleId: string | undefined,
        login: string | undefined,
        loginNormalized: string | undefined,
        email: string | undefined,
        emailNormalized: string | undefined,
        passwordHash: string | undefined,
        salt: string | undefined,
        isActive: boolean | undefined,
        isTemporaryPassword: boolean | undefined,
        roles?: UserRoleResponse[] | undefined
    ) {
        this.id = id;
        this.created = created;
        this.updated = updated;
        this.googleId = googleId;
        this.login = login;
        this.loginNormalized = loginNormalized;
        this.email = email;
        this.emailNormalized = emailNormalized;
        this.passwordHash = passwordHash;
        this.salt = salt;
        this.isActive = isActive;
        this.isTemporaryPassword = isTemporaryPassword;
        this.roles = roles;
    }
}
