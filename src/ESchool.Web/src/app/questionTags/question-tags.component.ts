import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AlertModule } from 'ng2-bootstrap';
import { ModalDirective } from 'ng2-bootstrap';
import { DialogService } from "ng2-bootstrap-modal";

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService } from './../shared/translate';
import { ConfirmDialogComponent } from './../shared/modals/confirm-dialog.component';

import { QuestionTag } from './question-tags.model';
import { AlertModel } from './../shared/models/alerts';
import { QuestionTagsService } from './question-tags.service';

@Component({
  selector: 'question-tags',
  templateUrl: './question-tags.component.html',
  styleUrls: [
    './question-tags.style.css'
  ]
})
export class QuestionTagsComponent implements OnInit {
  @ViewChild('childModal')
  public childModal: ModalDirective;
  private questionTag: QuestionTag;
  private editQuestionTag: QuestionTag;
  private questionTags: QuestionTag[];
  private alert: AlertModel;
  private DESCRIPTION: string;

  constructor(private questionTagsService: QuestionTagsService,
    private notificationService: NotificationService,
    private _translate: TranslateService,
    private dialogService: DialogService) {
  }

  ngOnInit() {
    this.questionTag = new QuestionTag();
    this.editQuestionTag = new QuestionTag();
    this.questionTags = [];
    this.alert = {
      type: '',
      message: ''
    };
    this.getQuestionTags();
    this.DESCRIPTION = this._translate.instant('DESCRIPTION');
  };

  getQuestionTags = () => {
    var self = this;
    self.questionTagsService.get()
      .subscribe((questionTags) => {
        self.questionTags = questionTags;
      },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  };

  addQuestionTag = () => {
    var self = this;
    self.questionTagsService.create(self.questionTag)
      .subscribe((id: number) => {
        self.questionTag.id = id;
        self.alert.type = 'success';
        self.alert.message = this._translate.instant('SAVED');
        self.getQuestionTags();
      },
      error => {
        self.notificationService.printErrorMessage('Failed to create question tag. ' + error);
      });
  };

  removeQTag = (qtag: QuestionTag) => {
    var self = this;
    this.dialogService.addDialog(ConfirmDialogComponent, {
      title: this._translate.instant('CONFIRMATION'),
      message: this._translate.instant('MSG_CONFIRM_DELETEING_QUESTION_TAGS'),
      confirmText: this._translate.instant('BUTTON_OK'),
      dismissText: this._translate.instant('BUTTON_CANCEL')
    })
      .subscribe((isConfirmed) => {
        if (isConfirmed) {
          self.questionTagsService.delete(qtag.id)
            .subscribe((questionTagCreated) => {
              self.alert.type = 'success';
              self.alert.message = this._translate.instant('SAVED');
              self.getQuestionTags();
            },
            error => {
              self.notificationService.printErrorMessage('Failed to delete question tag. ' + error);
            });
        }
      });
  };

  openEditDialog = (qtag: QuestionTag) => {
    this.questionTag = qtag;
  };

  updateQuestionTag = () => {
    var self = this;
    self.questionTagsService.update(self.editQuestionTag)
      .subscribe((questionTagCreated) => {
        self.alert.type = 'success';
        self.alert.message = this._translate.instant('SAVED');
        self.getQuestionTags();
        self.editQuestionTag = new QuestionTag();
        self.childModal.hide();
      },
      error => {
        self.notificationService.printErrorMessage('Failed to update question tag. ' + error);
      });
  };

  submitForm = (isValid: boolean) => {
    if (isValid) {
      this.addQuestionTag();
    }
  };

  showChildModal(qtag: QuestionTag): void {
    this.editQuestionTag = Object.assign({}, qtag);
    this.childModal.show();
  };

  cancelUpdate = () => {
    this.childModal.hide();
  };
}