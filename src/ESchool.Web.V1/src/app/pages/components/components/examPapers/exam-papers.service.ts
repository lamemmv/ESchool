import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ConfigService } from './../../../../shared/utils/config.service';
import { AppService } from './../../../../shared/app.service';
import { AuthService } from './../../../../security';

@Injectable()
export class ExamPapersService {
    private _baseUrl: string = '';
    constructor(private http: Http,
        private configService: ConfigService,
        private appService: AppService,
        private authService: AuthService) {
        this._baseUrl = configService.getAdminApiURI();
    }

    get(page: number, size: number) {
        let self = this, request = { page: page, size: size };
        return self.http.get(self._baseUrl + 'examPapers', { params: request,
        headers: this.authService.authFormHeaders() })
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }

    getById(id: number) {
        let self = this;
        let options = new RequestOptions({ headers: this.authService.authFormHeaders() });
        return self.http.get(self._baseUrl + 'examPapers/' + id, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }

    create(request: any) {
        let self = this;
        let options = new RequestOptions({ headers: this.authService.authFormHeaders() });
        return self.http.post(self._baseUrl + 'examPapers', request, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }

    update(request: any) {
        let self = this;
        let headers = new Headers({ 'Content-Type': 'application/json' }); // ... Set content type to JSON
        let options = new RequestOptions({ headers: headers }); // Create a request option
        return self.http.put(self._baseUrl + 'examPapers/' + request.id, request, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }

    delete(id: number) {
        let self = this;
        let options = new RequestOptions({ headers: this.authService.authFormHeaders() });
        return self.http.delete(self._baseUrl + 'examPapers/' + id, options)
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError);
    }
}