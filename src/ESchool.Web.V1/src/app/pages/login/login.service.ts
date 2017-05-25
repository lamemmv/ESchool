import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { AppService } from './../../shared/app.service';
import { ConfigService } from './../../shared/utils/config.service';

@Injectable()
export class LoginService {
    private _baseUrl: string = '';
    private _uploadFileUrl: string = '';
    constructor(private http: Http,
        configService: ConfigService,
        private appService: AppService) {
        this._baseUrl = configService.getApiURI();
    }
 
    login(request: any) {
        request.grant_type = 'password';
        request.client_id = 'ESchool.Web';
        request.client_secret = 'eschool.web.secret';
        const self = this;
        const headers = new Headers({ 'Content-Type': 'application/x-www-form-urlencoded' });
        const options = new RequestOptions({ headers: headers });

        return self.http.post(self._baseUrl + 'connect/token', $.param(request), options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }
}