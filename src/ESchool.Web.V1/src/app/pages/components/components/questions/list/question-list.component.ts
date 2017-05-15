import { Component, OnInit } from '@angular/core';
import { AlertModule } from 'ng2-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { NotificationService } from './../../../../../shared/utils/notification.service';
import { Question } from './../question.model';
import { QuestionsService } from './../questions.service';
@Component({
  selector: 'question-list',
  templateUrl: './question-list.component.html',
  styleUrls: [
    './question-list.style.scss'
  ]
})

export class QuestionListComponent implements OnInit {
  
  private questions: Question[] = [];

  constructor(private _translate: TranslateService,
    private questionsService: QuestionsService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.getQuestions();
  }

  getQuestions() {
    let self = this;
    this.questionsService.get().subscribe((questions) => {
      this.questions = questions;
    },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  }
}
