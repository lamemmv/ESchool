import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ConfigService } from './../shared/utils/config.service';
import { AppService } from './../shared/app.service';

@Injectable()
export class GroupsService {
    private _baseUrl: string = '';
    private _uploadFileUrl: string = '';
    constructor(private http: Http,
        private configService: ConfigService,
        private appService: AppService) {
        this._baseUrl = configService.getAdminApiURI();
    }

    get() {
        let self = this;
        return self.http.get(self._baseUrl + 'groups')
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    };
}