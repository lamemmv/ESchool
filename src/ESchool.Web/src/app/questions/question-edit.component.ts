import { Component, OnInit, ViewChildren, AfterViewChecked, Renderer, QueryList, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { AlertModule } from 'ng2-bootstrap';
import { RatingModule } from "ngx-rating";

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService } from './../shared/translate';
import { UtilitiesService } from './../shared/utils/utilities.service';
import { QuestionsService } from './questions.service';
import { QuestionTagsService } from './../questionTags/question-tags.service';
import { AlertModel } from './../shared/models/alerts';
import {
  Question, QTag, CreateQuestionModel,
  Answer, QuestionView, QuestionType,
  QuestionTypes
} from './question.model';
import { QuestionTag } from './../questionTags/question-tags.model';


@Component({
  selector: 'question-edit',
  templateUrl: './question-edit.component.html',
  styleUrls: [
    './question-edit.style.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class EditQuestionComponent implements OnInit, AfterViewChecked {
  @ViewChildren('answers') answerInputs: QueryList<any>;
  private alert: AlertModel;
  private question = new Question();
  private questionTags: QuestionTag[];
  private view = new QuestionView();
  private questionTypes: QuestionType[] = new Array();
  private answerName: string = 'A';
  private hasJustAddedAnswer: boolean = false;
  private selectedQtags: QTag[] = new Array();
  private questionId: number;
  constructor(private _translate: TranslateService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
    private questionTagsService: QuestionTagsService,
    private questionService: QuestionsService,
    private utilitiesService: UtilitiesService,
    private rd: Renderer) { }

  ngOnInit() {
    this.alert = {
      type: '',
      message: ''
    };

    let id = +this.route.snapshot.params['id'];
    this.questionId = id;
    if (id) {
      this.view.title = this._translate.instant('EDIT_QUESTION_TITLE');
      this.view.okText = this._translate.instant('UPDATE');
      this.getQuestion(id);
    } else {
      this.view.title = this._translate.instant('CREATE_QUESTION_TITLE');
      this.view.okText = this._translate.instant('BUTTON_SAVE');
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

  ngAfterViewChecked() {
    if (this.answerInputs && this.answerInputs.last && this.hasJustAddedAnswer) {
      this.rd.invokeElementMethod(this.answerInputs.last.nativeElement, 'focus');
      this.hasJustAddedAnswer = false;
    }
  };

  getQuestion(id: number) {
    var self = this;
    self.questionService.getById(id).subscribe((question) => {
      self.question = question;
      self.getSelectedQTags(self.question.qTags);
    },
      error => {
        self.notificationService.printErrorMessage('Failed to load question. ' + error);
      });
  };

  getSelectedQTags(qtags: QTag[]) {
    let self = this;
    qtags.forEach(qtag => {
      self.selectedQtags.push({ id: qtag.id, name: qtag.name });
    });
  };

  onReady(): void { };

  onChange(): void { };

  onFocus(): void { };

  onBlur(): void { };

  isValid(): boolean {
    if (!this.question.content) {
      return false;
    }

    if (this.question.answers.length == 0) {
      return false;
    }

    if (!this.question.answers.find(x=>x.dss == true)){
      return false;
    }
    
    return true;
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

  cancel(): void { this.router.navigate(['/questions']); };

  save(): void {
    let self = this, promise = null;
    let model = new CreateQuestionModel();
    model.content = self.question.content;
    model.description = self.question.description;
    model.type = self.question.type;
    model.answers = self.question.answers;
    model.difficultLevel = self.question.difficultLevel;
    model.id = self.questionId;
    self.question.qTags.forEach(qtag => {
      model.qTags.push(qtag.name);
    });

    if (self.questionId) {
      promise = self.questionService.update(model);
    } else {
      promise = self.questionService.create(model);
    }

    promise.subscribe((id: any) => {
      if (!self.questionId) {
        self.question.id = id;
      }

      self.alert.type = 'success';
      self.alert.message = self._translate.instant('SAVED');
      this.router.navigate(['/questions']);
    },
      error => {
        self.notificationService.printErrorMessage('Failed to create question. ' + error);
      });
  };

  addAnswer(): void {
    if (this.question.answers.length > 0) {
      this.answerName = this.utilitiesService.nextChar(this.answerName);
    }
    this.question.answers.push({ body: '', dss: false, answerName: this.answerName });
    this.hasJustAddedAnswer = true;
  };

  removeAnswer(answer: Answer): void {
    let index = this.question.answers.indexOf(answer);
    this.question.answers.splice(index, 1);
  };

  onItemAdded(item: QuestionTag) {
    this.selectedQtags.push(item);
    this.updateQTagsModel();
  };

  onItemSelected(item: QuestionTag) {
    console.log('onItemSelected: ' + item.name);
  }

  onItemRemoved(item: QuestionTag) {
    var self = this;
    let index = self.selectedQtags.indexOf(self.selectedQtags.find(i => i.name == item.name));
    self.selectedQtags.splice(index, 1);
    self.updateQTagsModel();
  };

  updateQTagsModel() {
    var self = this;
    this.selectedQtags.forEach((qtag: QuestionTag) => {
      if (!self.question.qTags.find(q => q.name == qtag.name)) {
        self.question.qTags.push({ id: qtag.id, name: qtag.name });
      }
    });
  };

  onClickRating(event: any){
    console.log('onClickRating: '+ event);
  };

  onRatingChange(event: any){
    console.log('onRatingChange: '+ event);
  };
}