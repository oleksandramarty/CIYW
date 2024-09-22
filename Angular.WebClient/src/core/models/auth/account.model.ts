export interface IJwtTokenResponse {
    token: string | undefined;
}

export class JwtTokenResponse implements IJwtTokenResponse {
    token: string | undefined;

    constructor(token: string | undefined) {
        this.token = token;
    }
}
