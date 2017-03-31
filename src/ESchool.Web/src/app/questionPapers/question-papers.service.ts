import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { ConfigService } from './../shared/utils/config.service';
import { AppService } from './../shared/app.service';

@Injectable()
export class QuestionPapersService {
  private _baseUrl: string = '';

  constructor(private http: Http, 
  private configService: ConfigService, 
  private appService: AppService) {
        this._baseUrl = configService.getAdminApiURI();
  }

    get(){
        var self = this;
        return this.http.get(this._baseUrl + 'questionPapers')
            .map((res: Response) => {
                return res.json();
            })
            .catch(self.appService.handleError); 
    };
}