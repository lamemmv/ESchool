import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AlertModule } from 'ng2-bootstrap';
import { ModalDirective } from 'ng2-bootstrap';
import { DialogService } from "ng2-bootstrap-modal";

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService }   from './../shared/translate';

import { AlertModel } from './../shared/models/alerts';
import { Question } from './question.model'

@Component({
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: [
    './questions.style.css',
    './../../plugins/datatables/dataTables.bootstrap.css'
  ]
})
export class QuestionsComponent implements OnInit {
  @ViewChild('childModal')
  public childModal: ModalDirective;
  private alert: AlertModel;
  private question = new Question();
  constructor(_translate: TranslateService) {

  }

  ngOnInit() {
    this.alert = {
      type: '',
      message: ''
    };
  };

}