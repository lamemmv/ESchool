import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ConfigService } from './../shared/utils/config.service';
import { AppService } from './../shared/app.service';

@Injectable()
export class QuestionsService {
    private _baseUrl: string = '';
    private _uploadFileUrl: string = '';
    constructor(private http: Http,
        private configService: ConfigService,
        private appService: AppService) {
        this._baseUrl = configService.getAdminApiURI();
    }

    getUploadFileUrl():string{
        return this._baseUrl + 'files';
    };

    get() {
        let self = this;
        return self.http.get(self._baseUrl + 'questions')
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    };

    getById(id: number) {
        let self = this;
        return self.http.get(self._baseUrl + 'questions/' + id)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    };

    create(request: any) {
        let self = this;
        return self.http.post(self._baseUrl + 'questions', request)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    };

    update(request: any){
        let self = this;
        let bodyString = JSON.stringify(request); // Stringify payload
        let headers = new Headers({ 'Content-Type': 'application/json' }); // ... Set content type to JSON
        let options = new RequestOptions({ headers: headers }); // Create a request option
        return self.http.put(self._baseUrl + 'questions/' + request.id, request, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    };

    delete(id: number) {
        let self = this;
        return self.http.delete(self._baseUrl + 'questions/' + id)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    };
}