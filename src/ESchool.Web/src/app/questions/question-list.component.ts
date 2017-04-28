import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { AlertModule } from 'ng2-bootstrap';
import { Modal } from 'ngx-modal';
import { DialogService } from "ng2-bootstrap-modal";

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService } from './../shared/translate';

import { AlertModel } from './../shared/models/alerts';
import { ConfirmDialogComponent } from './../shared/modals/confirm-dialog.component';
import { Question } from './question.model';
import { QuestionsService } from './questions.service';

@Component({
  selector: 'questions',
  templateUrl: './question-list.component.html',
  styleUrls: [
    './questions.style.css',
    './../../plugins/datatables/dataTables.bootstrap.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class QuestionListComponent implements OnInit {
  @ViewChild('childModal')
  public childModal: Modal;
  private alert: AlertModel;
  private questions: Question[];
  private question: Question = new Question();
  constructor(private _translate: TranslateService,
    private router: Router,
    private questionsService: QuestionsService,
    private notificationService: NotificationService,
    private dialogService: DialogService) {

  }

  ngOnInit() {
    this.alert = {
      type: '',
      message: ''
    };

    this.getQuestions();
  };

  getQuestions() {
    var self = this;
    this.questionsService.get().subscribe((questions) => {
      self.questions = questions;
    },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  };

  addQuestion(): void {
    this.router.navigate(['/admin/questions/create']);
  };

  removeQuestion(question: Question) {
    event.stopPropagation();
    var self = this;
    this.dialogService.addDialog(ConfirmDialogComponent, {
      title: this._translate.instant('QUESTION'),
      message: this._translate.instant('MSG_CONFIRM_DELETEING_QUESTION_TAGS'),
      confirmText: this._translate.instant('BUTTON_OK'),
      dismissText: this._translate.instant('BUTTON_CANCEL')
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          self.questionsService.delete(question.id)
            .subscribe((rowEffects: any) => {
              self.alert.type = 'success';
              self.alert.message = this._translate.instant('SAVED');
              self.getQuestions();
            },
            error => {
              self.notificationService.printErrorMessage('Failed to delete question tag. ' + error);
            });
        }
      });
  };

  editQuestion(question: Question) {
    this.router.navigate(['/admin/questions/edit', question.id]);
  };

  showChildModal(qt: Question): void {
    this.question = Object.assign({}, qt);
    this.childModal.open();
  };

  closeModal(): void {
    this.childModal.close();
  };
}