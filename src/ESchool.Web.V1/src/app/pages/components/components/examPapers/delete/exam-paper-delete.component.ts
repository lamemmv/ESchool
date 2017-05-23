import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ExamPapersService } from './../exam-papers.service';
import { NotificationService } from './../../../../../shared/utils/notification.service';
@Component({
  selector: 'exam-paper-delete',
  styleUrls: [('./exam-paper-delete.style.scss')],
  templateUrl: './exam-paper-delete.component.html'
})

export class ConfirmDeleteExamPaperComponent implements OnInit {

  modalHeader: string;
  modalContent: any;

  constructor(private activeModal: NgbActiveModal,
    private examPapersService: ExamPapersService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {

  }

  save() {
    const self = this;
    this.examPapersService.delete(this.modalContent.id)
      .subscribe((rowEffects: any) => {
        self.activeModal.close(true);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to delete exam paper. ' + error);
      });
  }

  cancel() {
    this.activeModal.dismiss();
  }

}
