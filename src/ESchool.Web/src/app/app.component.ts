import { Component } from '@angular/core';

import { TranslateService } from './shared/translate';

@Component({
  selector: 'eschool-app',
  templateUrl: './app.component.html',
})
export class AppComponent  { 
  constructor(private _translate: TranslateService){
        this._translate.use('vi');
    }
 }
