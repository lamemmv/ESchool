import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions, RequestMethod } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { ConfigService } from './../../shared/utils/config.service';
import { AppService } from './../../shared/app.service';
import * as $ from 'jquery';
import { OidcSecurityService, AuthWellKnownEndpoints } from './../../security/auth';
import { AuthService } from './../../security';

@Injectable()
export class LoginService {
    private baseUrl: string = '';
    constructor(configService: ConfigService,
        private http: Http,
        private appService: AppService,
        private securityService: OidcSecurityService,
        private authWellKnownEndpoints: AuthWellKnownEndpoints,
        private authService: AuthService) {
        this.baseUrl = configService.getAdminApiURI();
    }

    authenticate() {
        return this.http.get(this.baseUrl)
            .map((res: Response) => {
                return res.json();
            })
            .catch(this.appService.handleError);
    }

    login(request: any) {
        return this.securityService.authorize(request);
    }

    test() {
        let self = this;
        let options = new RequestOptions({
            method: RequestMethod.Get,
            headers: this.authService.authFormHeaders()/*, 
            withCredentials: true*/
        });
        return self.http.get(this.baseUrl + "users/1", options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }
}