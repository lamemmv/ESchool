import { Injectable, EventEmitter, Output } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import { ConfigService } from './../../shared/utils/config.service';
import { OidcSecurityCommon } from './oidc.security.common';

@Injectable()
export class AuthWellKnownEndpoints {

    @Output() onWellKnownEndpointsLoaded: EventEmitter<any> = new EventEmitter<any>(true);

    issuer: string;
    jwksUri: string;
    authorizationEndpoint: string;
    tokenEndpoint: string;
    userinfoEndpoint: string;
    endSessionEndpoint: string;
    checkSessionIframe: string;
    revocationEndpoint: string;
    introspectionEndpoint: string;

    constructor(
        private http: Http,
        private authConfiguration: ConfigService,
        private oidcSecurityCommon: OidcSecurityCommon
    ) {
    }

    setupModule() {
        let data = this.oidcSecurityCommon.retrieve(this.oidcSecurityCommon.storage_well_known_endpoints);
        this.oidcSecurityCommon.logDebug(data);
        if (data && data !== '') {
            this.oidcSecurityCommon.logDebug('AuthWellKnownEndpoints already defined');
            this.issuer = data.issuer;
            this.jwksUri = data.jwks_uri;
            this.authorizationEndpoint = data.authorization_endpoint;
            this.tokenEndpoint = data.token_endpoint;
            this.userinfoEndpoint = data.userinfo_endpoint;

            if (data.end_session_endpoint) {
                this.endSessionEndpoint = data.end_session_endpoint;
            };

            if (data.check_session_iframe) {
                this.checkSessionIframe = data.check_session_iframe;
            };

            if (data.revocation_endpoint) {
                this.revocationEndpoint = data.revocation_endpoint;
            };

            if (data.introspection_endpoint) {
                this.introspectionEndpoint = data.introspection_endpoint;
            }

            this.onWellKnownEndpointsLoaded.emit();
        } else {
            this.oidcSecurityCommon.logDebug('AuthWellKnownEndpoints first time, get from the server');
            this.getWellKnownEndpoints()
                .subscribe((data: any) => {
                    this.issuer = data.issuer;
                    this.jwksUri = data.jwks_uri;
                    this.authorizationEndpoint = data.authorization_endpoint;
                    this.tokenEndpoint = data.token_endpoint;
                    this.userinfoEndpoint = data.userinfo_endpoint;

                    if (data.end_session_endpoint) {
                        this.endSessionEndpoint = data.end_session_endpoint;
                    };

                    if (data.check_session_iframe) {
                        this.checkSessionIframe = data.check_session_iframe;
                    };

                    if (data.revocation_endpoint) {
                        this.revocationEndpoint = data.revocation_endpoint;
                    };

                    if (data.introspection_endpoint) {
                        this.introspectionEndpoint = data.introspection_endpoint;
                    }

                    this.oidcSecurityCommon.store(this.oidcSecurityCommon.storage_well_known_endpoints, data);
                    this.oidcSecurityCommon.logDebug(data);

                    this.onWellKnownEndpointsLoaded.emit();
                });
        }
    }

    private getWellKnownEndpoints = (): Observable<any> => {

        let headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded; charset=utf-8');
        headers.append('Accept', 'application/json');

        let url = this.authConfiguration.getApiURI() + '.well-known/openid-configuration';

        return this.http.get(url, {
            headers: headers,
            body: ''
        }).map((res: any) => res.json());
    }
}