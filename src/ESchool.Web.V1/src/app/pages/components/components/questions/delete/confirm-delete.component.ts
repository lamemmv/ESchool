import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { QuestionsService } from './../questions.service';
import { NotificationService } from './../../../../../shared/utils/notification.service';
@Component({
  selector: 'question-confirm-delete',
  styleUrls: [('./confirm-delete.style.scss')],
  templateUrl: './confirm-delete.component.html'
})

export class ConfirmDeleteQuestionComponent implements OnInit {

  modalHeader: string;
  modalContent: any;

  constructor(private activeModal: NgbActiveModal,
    private questionsService: QuestionsService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {

  }

  save() {
    const self = this;
    this.questionsService.delete(this.modalContent.question.id)
      .subscribe((rowEffects: any) => {
        self.activeModal.close(true);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to delete question tag. ' + error);
      });
  }

  cancel() {
    this.activeModal.dismiss();
  }

}
