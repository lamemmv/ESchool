import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ConfigService } from './../../../../shared/utils/config.service';
import { AppService } from './../../../../shared/app.service';
import { AuthService } from './../../../../security';

@Injectable()
export class QuestionTagsService {
    private _baseUrl: string = '';

    constructor(private http: Http,
        private configService: ConfigService,
        private appService: AppService,
        private authService: AuthService) {
        this._baseUrl = configService.getAdminApiURI();
    }

    get = (groupId: number) => {
        let self = this;
        let options = new RequestOptions({ headers: this.authService.authFormHeaders() });
        return this.http.get(this._baseUrl + 'qtags/getByGroup/' + groupId, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }

    getById = (id: number) => {
        let self = this;
        let options = new RequestOptions({ headers: this.authService.authFormHeaders() });
        return this.http.get(this._baseUrl + 'qtags/' + id, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }
    
    create = (qtag: any) => {
        let self = this;
        let options = new RequestOptions({ headers: this.authService.authFormHeaders() });
        return this.http.post(this._baseUrl + 'qtags', qtag, options)
            .map((res: Response) => {
                if (res.status != 201 && res.status != 200){
                    self.appService.handleError(res);
                }
                return res.json();
            })
            .catch(self.appService.handleError);
    }

    update = (qtag: any) => {
        let self = this;
        let headers = new Headers({ 'Content-Type': 'application/json' }); // ... Set content type to JSON
        let options = new RequestOptions({ headers: headers }); // Create a request option

        return this.http.put(this._baseUrl + 'qtags/' + qtag.id, qtag, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }

    delete = (id: number) => {
        let self = this;
        let options = new RequestOptions({ headers: this.authService.authFormHeaders() });
        return this.http.delete(this._baseUrl + 'qtags/' + id, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }
}
