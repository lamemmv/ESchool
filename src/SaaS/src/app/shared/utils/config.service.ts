import { Injectable } from '@angular/core';

export class DefaultConfiguration {
    logConsoleWarningActive = true;
    logConsoleDebugActive = false;
    silentRenew = true;
    startCheckSession = false;
    clientId = 'netpower.qms.saas.client';
    responseType = 'id_token token';
    maxIdTokenIatOffsetAllowedInSeconds = 3;
    startupRoute = '/dataeventrecords';
    // HTTP 403
    forbiddenRoute = '/forbidden';
    // HTTP 401
    unauthorizedRoute = '/unauthorized';
    postLogoutRedirectUri = 'https://localhost:53090/unauthorized';
    scope = 'openid email profile';
    redirectUrl = 'https://localhost:53090';
}

export class OpenIDConfiguration {
    logConsoleWarningActive: boolean;
    logConsoleDebugActive: boolean;
    silentRenew: boolean;
    startCheckSession: boolean;
    clientId: string;
    responseType: string;
    maxIdTokenIatOffsetAllowedInSeconds: number;
    startupRoute: string;
    forbiddenRoute: string;
    unauthorizedRoute: string;
    postLogoutRedirectUri: string;
    scope: string;
    redirectUrl: string;
}

@Injectable()
export class ConfigService {

    _apiURI: string;
    _adminApiURI: string;
    _pageSize: number = 10;
    private openIDConfiguration: OpenIDConfiguration;
    private defaultConfiguration = new DefaultConfiguration();
    constructor() {
        this._apiURI = 'http://localhost:53090/';
        this._adminApiURI = 'http://localhost:53090/admin/';
    }

    init(openIDConfiguration: OpenIDConfiguration) {
        this.openIDConfiguration = openIDConfiguration;
    }

    getApiURI() {
        return this._apiURI;
    }

    getApiHost() {
        return this._apiURI.replace('api/', '');
    }

    getAdminApiURI() {
        return this._adminApiURI;
    }

    getPageSize(): number {
        return this._pageSize;
    }

    get logConsoleWarningActive(): boolean {
        return this.openIDConfiguration.logConsoleWarningActive || this.defaultConfiguration.logConsoleWarningActive;
    }

    get logConsoleDebugActive(): boolean {
        return this.openIDConfiguration.logConsoleDebugActive || this.defaultConfiguration.logConsoleDebugActive;
    }

    get silentRenew(): boolean {
        return this.openIDConfiguration.silentRenew || this.defaultConfiguration.silentRenew;
    }

    get startCheckSession(): boolean {
        return this.openIDConfiguration.startCheckSession || this.defaultConfiguration.startCheckSession;
    }

    get clientId(): string {
        return this.openIDConfiguration.clientId || this.defaultConfiguration.clientId;
    }

    get responseType(): string {
        return this.openIDConfiguration.responseType || this.defaultConfiguration.responseType;
    }

    get maxIdTokenIatOffsetAllowedInSeconds(): number {
        return this.openIDConfiguration.maxIdTokenIatOffsetAllowedInSeconds || this.defaultConfiguration.maxIdTokenIatOffsetAllowedInSeconds;
    }

    get startupRoute(): string {
        return this.openIDConfiguration.startupRoute || this.defaultConfiguration.startupRoute;
    }

    get forbiddenRoute(): string {
        return this.openIDConfiguration.forbiddenRoute || this.defaultConfiguration.forbiddenRoute;
    }

    get unauthorizedRoute(): string {
        return this.openIDConfiguration.unauthorizedRoute || this.defaultConfiguration.unauthorizedRoute;
    }

    get scope(): string {
        return this.openIDConfiguration.scope || this.defaultConfiguration.scope;
    }

    get postLogoutRedirectUri(): string {
        return this.openIDConfiguration.postLogoutRedirectUri || this.defaultConfiguration.postLogoutRedirectUri;
    }

    get redirectUrl(): string {
        return this.openIDConfiguration.redirectUrl || this.defaultConfiguration.redirectUrl;
    }
}
