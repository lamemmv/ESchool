import {  Component } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TranslateService } from './../shared/translate';
@Component({
  templateUrl:  './questions.component.html'
})
export class QuestionsComponent {
  constructor(private _translate: TranslateService){
    this._translate.use('vi');
  }
 }