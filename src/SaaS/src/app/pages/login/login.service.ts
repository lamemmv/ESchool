import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { ConfigService } from './../../shared/utils/config.service';
import { AppService } from './../../shared/app.service';
import * as $ from 'jquery';

@Injectable()
export class LoginService {
    private baseUrl: string = '';
    constructor(configService: ConfigService,
        private http: Http,
        private appService: AppService) {
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
        request.grant_type = 'password';
        request.client_id = 'qms.saas.client';
        request.client_secret = 'qms.saas.secret';
        const headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        const options = new RequestOptions({ headers: headers });

        return this.http.post(this.baseUrl + 'connect/token', $.param(request), options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(this.appService.handleError);
    }
}