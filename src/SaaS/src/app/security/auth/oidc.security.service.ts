import { Injectable, EventEmitter, Output } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { Observable } from 'rxjs/Rx';
import { Router } from '@angular/router';
import { ConfigService, OpenIDConfiguration } from './../../shared/utils/config.service';
import { OidcSecurityValidation } from './oidc.security.validation';
import { OidcSecurityCheckSession } from './oidc.security.check-session';
import { OidcSecuritySilentRenew } from './oidc.security.silent-renew';
import { OidcSecurityUserService } from './oidc.security.user-service';
import { OidcSecurityCommon } from './oidc.security.common';
import { AuthWellKnownEndpoints } from './auth.well-known-endpoints';

@Injectable()
export class OidcSecurityService {

    @Output() onUserDataLoaded: EventEmitter<any> = new EventEmitter<any>(true);

    checkSessionChanged: boolean;
    isAuthorized: boolean;

    private headers: Headers;
    private oidcSecurityValidation: OidcSecurityValidation;
    private errorMessage: string;
    private authWellKnownEndpointsLoaded = false;

    constructor(
        private http: Http,
        private authConfiguration: ConfigService,
        private router: Router,
        private oidcSecurityCheckSession: OidcSecurityCheckSession,
        private oidcSecuritySilentRenew: OidcSecuritySilentRenew,
        private oidcSecurityUserService: OidcSecurityUserService,
        private oidcSecurityCommon: OidcSecurityCommon,
        private authWellKnownEndpoints: AuthWellKnownEndpoints
    ) {
    }

    setupModule(openIDConfiguration: OpenIDConfiguration) {

        this.authConfiguration.init(openIDConfiguration);
        this.oidcSecurityValidation = new OidcSecurityValidation(this.oidcSecurityCommon);

        this.oidcSecurityCheckSession.onCheckSessionChanged.subscribe(() => { this.onCheckSessionChanged(); });
        this.authWellKnownEndpoints.onWellKnownEndpointsLoaded.subscribe(() => { this.onWellKnownEndpointsLoaded(); });

        this.oidcSecurityCommon.setupModule();
        this.oidcSecurityUserService.setupModule();

        this.headers = new Headers();
        this.headers.append('Content-Type', 'application/json');
        this.headers.append('Accept', 'application/json');

        if (this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_is_authorized) !== '') {
            this.isAuthorized = this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_is_authorized);
        }

        this.oidcSecurityCommon.logDebug('STS server: ' + this.authConfiguration.getApiURI());
        this.authWellKnownEndpoints.setupModule();

        if (this.authConfiguration.silentRenew) {
            this.oidcSecuritySilentRenew.initRenew();
        }

        if (this.authConfiguration.startCheckSession) {
            this.oidcSecurityCheckSession.init().subscribe(() => {
                this.oidcSecurityCheckSession.pollServerSession(this.authConfiguration.clientId);
            });
        }
    }

    getToken(): any {
        let token = this.oidcSecurityCommon.getAccessToken();
        return decodeURIComponent(token);
    }

    getUserData(): any {
        if (!this.isAuthorized) {
            this.oidcSecurityCommon.logError('User must be logged in before you can get the user data!')
        }

        if (!this.oidcSecurityUserService.userData) {
            this.oidcSecurityUserService.userData = this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_user_data);
        }

        return this.oidcSecurityUserService.userData;
    }

    authorize(request: any) {

        let data = this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_well_known_endpoints);
        if (data && data !== '') {
            this.authWellKnownEndpointsLoaded = true;
        }

        if (!this.authWellKnownEndpointsLoaded) {
            this.oidcSecurityCommon.logError('Well known endpoints must be loaded before user can login!')
            return;
        }

        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');
        headers.append('Accept', 'application/json');
        let options = new RequestOptions({ headers: headers });
        //request.grant_type = 'client_credentials';
        request.grant_type = 'password';
        request.scope = 'netpower.qms.saas.api.read';
        request.client_id = 'netpower.qms.saas.client';
        request.client_secret = 'superSecretPassword';
        let self = this;
        return this.http.post(self.authWellKnownEndpoints.tokenEndpoint, $.param(request), options)
            .map((res: any) => {
                return res.json();
            })
            .catch(self.handleError1);
    }

    handleError1(error: Response | any) {
        return Observable.throw(error);
    }

    setStorage(storage: any) {
        this.oidcSecurityCommon.storage = storage;
    }

    logoff() {
        // /connect/endsession?id_token_hint=...&post_logout_redirect_uri=https://myapp.com
        this.oidcSecurityCommon.logDebug('BEGIN Authorize, no auth data');

        if (this.authWellKnownEndpoints.endSessionEndpoint) {
            let authorizationEndsessionUrl = this.authWellKnownEndpoints.endSessionEndpoint;

            let id_token_hint = this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_id_token);
            let post_logout_redirect_uri = this.authConfiguration.postLogoutRedirectUri;

            let url =
                authorizationEndsessionUrl + '?' +
                'id_token_hint=' + encodeURI(id_token_hint) + '&' +
                'post_logout_redirect_uri=' + encodeURI(post_logout_redirect_uri);

            this.resetAuthorizationData(false);

            if (this.authConfiguration.startCheckSession && this.checkSessionChanged) {
                this.oidcSecurityCommon.logDebug('only local login cleaned up, server session has changed');
            } else {
                window.location.href = url;
            }
        } else {
            this.resetAuthorizationData(false);
            this.oidcSecurityCommon.logDebug('only local login cleaned up, no end_session_endpoint');
        }
    }

    private successful_validation() {
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_auth_nonce, '');
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_auth_state_control, '');
        this.oidcSecurityCommon.logDebug('AuthorizedCallback token(s) validated, continue');
    }

    private refreshSession() {
        this.oidcSecurityCommon.logDebug('BEGIN refresh session Authorize');

        let nonce = 'N' + Math.random() + '' + Date.now();
        let state = Date.now() + '' + Math.random();

        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_auth_state_control, state);
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_auth_nonce, nonce);
        this.oidcSecurityCommon.logDebug('RefreshSession created. adding myautostate: ' + this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_auth_state_control));

        let url = this.createAuthorizeUrl(nonce, state);

        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_silent_renew_running, 'running');
        this.oidcSecuritySilentRenew.startRenew(url);
    }

    private setAuthorizationData(access_token: any, id_token: any) {
        if (this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_access_token) !== '') {
            this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_access_token, '');
        }

        this.oidcSecurityCommon.logDebug(access_token);
        this.oidcSecurityCommon.logDebug(id_token);
        this.oidcSecurityCommon.logDebug('storing to storage, getting the roles');
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_access_token, access_token);
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_id_token, id_token);
        this.isAuthorized = true;
        this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_is_authorized, true);
    }

    private createAuthorizeUrl(nonce: string, state: string): string {

        let authorizationUrl = this.authWellKnownEndpoints.authorizationEndpoint;
        let client_id = this.authConfiguration.clientId;
        let redirect_uri = this.authConfiguration.redirectUrl;
        let response_type = this.authConfiguration.responseType;
        let scope = this.authConfiguration.scope;

        let url =
            authorizationUrl + '?' +
            'response_type=' + encodeURI(response_type) + '&' +
            'client_id=' + encodeURI(client_id) + '&' +
            'redirect_uri=' + encodeURI(redirect_uri) + '&' +
            'scope=' + encodeURI(scope) + '&' +
            'nonce=' + encodeURI(nonce) + '&' +
            'state=' + encodeURI(state);

        return url;

    }

    private resetAuthorizationData(isRenewProcess: boolean) {
        if (!isRenewProcess) {
            this.isAuthorized = false;
            this.oidcSecurityCommon.resetStorageData(isRenewProcess);
            this.checkSessionChanged = false;
        }
    }

    handleError(error: any) {
        this.oidcSecurityCommon.logError(error);
        if (error.status == 403) {
            this.router.navigate([this.authConfiguration.forbiddenRoute]);
        } else if (error.status == 401) {
            let silentRenew = this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_silent_renew_running);
            this.resetAuthorizationData(silentRenew);
            this.router.navigate([this.authConfiguration.unauthorizedRoute]);
        }
    }

    private onCheckSessionChanged() {
        this.oidcSecurityCommon.logDebug('onCheckSessionChanged');
        this.checkSessionChanged = true;
    }

    private onWellKnownEndpointsLoaded() {
        this.oidcSecurityCommon.logDebug('onWellKnownEndpointsLoaded');
        this.authWellKnownEndpointsLoaded = true;
    }

    private extractData(res: Response) {
        let body = res.json();
        return body;
    }

    private handleErrorGetSigningKeys(error: Response | any) {
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
        return Observable.throw(errMsg);
    }

    private runTokenValidatation() {
        let source = Observable.timer(3000, 3000)
            .timeInterval()
            .pluck('interval')
            .take(10000);

        let subscription = source.subscribe(() => {
            if (this.isAuthorized) {
                if (this.oidcSecurityValidation.isTokenExpired(this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_id_token))) {
                    this.oidcSecurityCommon.logDebug('IsAuthorized: id_token isTokenExpired, start silent renew if active');

                    if (this.authConfiguration.silentRenew) {
                        this.refreshSession();
                    } else {
                        this.resetAuthorizationData(false);
                    }
                }
            }
        },
            (err: any) => {
                this.oidcSecurityCommon.logError('Error: ' + err);
            },
            () => {
                this.oidcSecurityCommon.logDebug('Completed');
            });
    }
}