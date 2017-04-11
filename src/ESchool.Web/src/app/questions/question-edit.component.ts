import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { AlertModule } from 'ng2-bootstrap';

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService } from './../shared/translate';
import { QuestionsService } from './questions.service';
import { QuestionTagsService } from './../questionTags/question-tags.service';
import { AlertModel } from './../shared/models/alerts';
import { Question, Answer, QuestionView, QuestionType, QuestionTypes } from './question.model';
import { QuestionTag } from './../questionTags/question-tags.model';


@Component({
  selector: 'question-edit',
  templateUrl: './question-edit.component.html',
  styleUrls: [
    './question-edit.style.css'
  ]
})
export class EditQuestionComponent implements OnInit {
  private alert: AlertModel;
  private question = new Question();
  private questionTags: QuestionTag[];
  private view = new QuestionView();
  private questionTypes: QuestionType[] = new Array();
  constructor(private _translate: TranslateService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
    private questionTagsService: QuestionTagsService,
    private questionService: QuestionsService) { }

  ngOnInit() {
    this.alert = {
      type: '',
      message: ''
    };

    let id = +this.route.snapshot.params['id'];
    if (id) {
      this.view.title = this._translate.instant('EDIT_QUESTION_TITLE');
    } else {
      this.view.title = this._translate.instant('CREATE_QUESTION_TITLE');
    }

    this.getQuestionTags();

    this.questionTypes.push({
      id: QuestionTypes.SingleChoice,
      name: this._translate.instant('ENUM_QUESTION_TYPES_SINGLE_CHOICE')
    }, {
        id: QuestionTypes.MultipleChoice,
        name: this._translate.instant('ENUM_QUESTION_TYPES_MULTIPLE_CHOICES')
      }
    );
  };

  onReady(): void { };

  onChange(): void { };

  onFocus(): void { };

  onBlur(): void { };

  isValid(): boolean {
    if (!this.question.content) {
      return false;
    }
    return true;
  };

  getQuestionTags = () => {
    var self = this;
    self.questionTagsService.get()
      .subscribe((questionTags) => {
        self.questionTags = questionTags;
        self.decorateQuestionTags(self.questionTags);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  };

  decorateQuestionTags(questionTags: QuestionTag[]) {
    questionTags.forEach(item => item.text = item.name);
  };

  public selected(qtag: QuestionTag): void {
  }

  public removed(qtag: QuestionTag): void {
  }

  public refreshValue(qtags: QuestionTag[]): void {
    this.question.qTagIds = new Array();
    qtags.forEach((qtag) => {
      this.question.qTagIds.push(qtag.id);
    });
  }

  cancel(): void { this.router.navigate(['/questions']); };

  save(): void {
    var self = this;
    /*self.questionService.create(self.question)
      .subscribe((id: number) => {
        self.question.id = id;
        self.alert.type = 'success';
        self.alert.message = self._translate.instant('SAVED');
      },
      error => {
        self.notificationService.printErrorMessage('Failed to create question. ' + error);
      });*/
  };

  addAnswer(): void {
    this.question.answers.push({ body: '', dss: false, answerName: '' });
  };

  removeAnswer(answer: Answer): void { 
    let index = this.question.answers.indexOf(answer);
    this.question.answers.splice(index, 1);
  };
}