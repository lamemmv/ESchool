import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { ConfigService } from './../../shared/utils/config.service';
import { AppService } from './../../shared/app.service';
import * as $ from 'jquery';
import { OidcSecurityService } from './../../security/auth';

@Injectable()
export class LoginService {
    private baseUrl: string = '';
    constructor(configService: ConfigService,
        private http: Http,
        private appService: AppService,
        private securityService: OidcSecurityService) {
        this.baseUrl = configService.getApiURI();
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
}