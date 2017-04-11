import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AlertModule } from 'ng2-bootstrap';

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService } from './../shared/translate';

import { AlertModel } from './../shared/models/alerts';
import { Question } from './question.model';

@Component({
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: [
    './questions.style.css',
    './../../plugins/datatables/dataTables.bootstrap.css'
  ]
})
export class QuestionsComponent implements OnInit {
  private alert: AlertModel;
  private questions : Question[];
  constructor(_translate: TranslateService,
    private router: Router) {

  }

  ngOnInit() {
    this.alert = {
      type: '',
      message: ''
    };
  };

  addQuestion(): void {
    this.router.navigate(['/question/create']);
   };
}