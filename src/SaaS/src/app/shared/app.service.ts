import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class AppService {
    handleError(error: Response | any) {
        return Observable.throw(error);
    }
}