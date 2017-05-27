import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { AppService } from './../../shared/app.service';
import { ConfigService } from './../../shared/utils/config.service';

@Injectable()
export class ForgotPasswordService {
    private _baseUrl: string = '';
    private _uploadFileUrl: string = '';
    constructor(private http: Http,
        configService: ConfigService,
        private appService: AppService) {
        this._baseUrl = configService.getApiURI();
    }
 
    forgotPassword(request: any) {
        const self = this;
        return self.http.post(self._baseUrl + 'api/accounts/forgotpassword', request)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }
}