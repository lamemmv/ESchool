import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import { TranslateService } from './translate';

@Injectable()
export class AppService {
    constructor(private http: Http) {

    }

    handleError(error: Response | any) {
        /*let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = body.error || JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }

        if (error.headers) {
            var applicationError = error.headers.get('Application-Error');
            var serverError = error.json();
            var modelStateErrors: string = '';

            if (!serverError.type) {
                console.log(serverError);
                for (var key in serverError) {
                    if (serverError[key])
                        modelStateErrors += serverError[key] + '\n';
                }
            }

            modelStateErrors = modelStateErrors = '' ? null : modelStateErrors;

            return Observable.throw(errMsg || applicationError || modelStateErrors || 'Server error');
        } else {
            return Observable.throw(errMsg);
        }*/
        return Observable.throw(error);
    };
}