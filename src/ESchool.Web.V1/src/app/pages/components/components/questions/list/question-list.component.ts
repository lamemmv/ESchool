import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertModule } from 'ng2-bootstrap';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { NotificationService } from './../../../../../shared/utils/notification.service';
import { Question, PagedList } from './../question.model';
import { QuestionsService } from './../questions.service';
import { AlertModel } from './../../../../../shared/models/alert';
import { ConfirmDeleteQuestionComponent } from './../delete/confirm-delete.component';
import { ConfigService } from './../../../../../shared/utils/config.service';

@Component({
  selector: 'question-list',
  templateUrl: './question-list.component.html',
  styleUrls: [
    './question-list.style.scss',
  ]
})

export class QuestionListComponent implements OnInit {

  private question = new PagedList();
  private alert: AlertModel;

  constructor(private router: Router,
    private questionsService: QuestionsService,
    private notificationService: NotificationService,
    private modalService: NgbModal,
    configService: ConfigService) {
    this.question.page = 1;
    this.question.size = configService.getPageSize();
  }

  ngOnInit() {
    this.alert = {
      type: '',
      message: '',
    };
    this.getQuestions(this.question.page);
  }

  getQuestions(page: number) {
    const self = this;
    this.questionsService.get(self.question.page, self.question.size).subscribe((response: PagedList) => {
      this.question = response;
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
    this.openDialog(question);
  }

  openDialog(content: Question) {
    const self = this;
    const activeModal = this.modalService.open(ConfirmDeleteQuestionComponent, {
      size: 'sm',
      backdrop: 'static',
    });
    activeModal.componentInstance.modalContent = content;
    activeModal.result.then((result) => {
      self.handleDialogClose(result);
    }, (reason) => {
      self.handleDialogClose(null);
    });
  }

  handleDialogClose(result) {
    if (result) {
      this.getQuestions(this.question.page);
    }
  }

  onPageChange(page: number) {
    this.getQuestions(page);
  }
}
