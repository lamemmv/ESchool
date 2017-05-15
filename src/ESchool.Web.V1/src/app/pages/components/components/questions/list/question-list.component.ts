import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertModule } from 'ng2-bootstrap';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';

import { NotificationService } from './../../../../../shared/utils/notification.service';
import { Question, PagedList } from './../question.model';
import { QuestionsService } from './../questions.service';
import { AlertModel } from './../../../../../shared/models/alert';
import { ConfirmDialogComponent } from './../../../../../shared/modals/confirm-dialog.component';
@Component({
  selector: 'question-list',
  templateUrl: './question-list.component.html',
  styleUrls: [
    './question-list.style.scss'
  ]
})

export class QuestionListComponent implements OnInit {

  private questions: Question[] = [];
  private alert: AlertModel;
  private questionListTranslation = {
    questionTitle: '',
    msgConfirmDelete: '',
    okButton: '',
    cancelButton: ''
  };

  constructor(private _translate: TranslateService,
    private router: Router,
    private questionsService: QuestionsService,
    private notificationService: NotificationService,
    private modalService: NgbModal) {
  }

  ngOnInit() {
    this.alert = {
      type: '',
      message: ''
    };
    this.getQuestions();
    this._translate.get(['REMOVE_QUESTION_TITLE',
      'MSG_CONFIRM_DELETEING_QUESTION_TAGS',
      'BUTTON_OK',
      'BUTTON_CANCEL']).subscribe((res: any) => {
        this.questionListTranslation.questionTitle = res.REMOVE_QUESTION_TITLE;
        this.questionListTranslation.msgConfirmDelete = res.MSG_CONFIRM_DELETEING_QUESTION_TAGS;
        this.questionListTranslation.okButton = res.BUTTON_OK;
        this.questionListTranslation.cancelButton = res.BUTTON_CANCEL;
      });
  }

  getQuestions() {
    let self = this;
    this.questionsService.get().subscribe((response: PagedList) => {
      this.questions = response.data;
    },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  }

  addQuestion(): void {
    this.router.navigate(['/pages/components/questions/create']);
  }

  editQuestion(question: Question) {
    this.router.navigate(['/pages/components/questions/edit', question.id]);
  }

  removeQuestion(question: Question) {
    event.stopPropagation();
    // let self = this;
    // const activeModal = this.modalService.open(EditQuestionTagComponent, {
    //   size: 'lg',
    //   backdrop: 'static'
    // });
    // activeModal.componentInstance.modalHeader = header;
    // activeModal.componentInstance.modalContent = content;
    // activeModal.result.then((result) => {
    //   self.handleDialogClose(result);
    // }, (reason) => {
    //   self.handleDialogClose(null);
    // });

    // this.dialogService.addDialog(ConfirmDialogComponent, {
    //   title: this.questionListTranslation.questionTitle,
    //   message: this.questionListTranslation.msgConfirmDelete,
    //   confirmText: this.questionListTranslation.okButton,
    //   dismissText: this.questionListTranslation.cancelButton
    // })
    //   .subscribe((isConfirmed) => {
    //     if (isConfirmed) {
    //       self.questionsService.delete(question.id)
    //         .subscribe((rowEffects: any) => {
    //           self.alert.type = 'success';
    //           self.alert.message = this._translate.instant('SAVED');
    //           self.getQuestions();
    //         },
    //         error => {
    //           self.notificationService.printErrorMessage('Failed to delete question tag. ' + error);
    //         });
    //     }
    //   });
  }
}
