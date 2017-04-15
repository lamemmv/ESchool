import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ConfigService } from './../shared/utils/config.service';
import { AppService } from './../shared/app.service';

@Injectable()
export class QuestionsService {
    private _baseUrl: string = '';

    constructor(private http: Http,
        private configService: ConfigService,
        private appService: AppService) {
        this._baseUrl = configService.getAdminApiURI();
    }

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
        return self.http.put(self._baseUrl + 'questions/' + request.id, request)
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