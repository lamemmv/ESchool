import {
  Component, OnInit, ViewChild, ViewChildren, AfterViewChecked,
  AfterViewInit, Renderer, QueryList, ViewEncapsulation, NgZone
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, Params } from '@angular/router';

import { AlertModule, ModalDirective } from 'ng2-bootstrap';
import { Modal } from 'ngx-modal';
import { RatingModule } from "ngx-rating";
import { CKButtonDirective, CKEditorComponent } from 'ng2-ckeditor';
import { NodeEvent, Ng2TreeSettings } from 'ng2-tree';

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService } from './../shared/translate';
import { UtilitiesService } from './../shared/utils/utilities.service';
import { QuestionsService } from './questions.service';
import { QuestionTagsService } from './../questionTags/question-tags.service';
import { AlertModel } from './../shared/models/alerts';
import {
  Question, QTag,
  Answer, QuestionView, QuestionType,
  QuestionTypes, FormFile, ESTreeNode
} from './question.model';
import { QuestionTag } from './../questionTags/question-tags.model';

declare var CKEDITOR: any;
@Component({
  selector: 'question-edit',
  templateUrl: './question-edit.component.html',
  styleUrls: [
    './question-edit.style.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class EditQuestionComponent implements OnInit, AfterViewChecked, AfterViewInit {
  @ViewChildren('answers') answerInputs: QueryList<any>;
  @ViewChild('uploadModal') public uploadModal: ModalDirective;
  @ViewChild("fileInput") fileInput: any;
  private alert: AlertModel;
  private question = new Question();
  private questionTags: QuestionTag[];
  private view = new QuestionView();
  private questionTypes: QuestionType[] = new Array();
  private answerName: string = 'A';
  private hasJustAddedAnswer: boolean = false;
  private selectedQtags: QTag[] = new Array();
  private questionId: number;
  private editor: any;
  private file = new FormFile();
  private tree: ESTreeNode;
  private treeSetting: Ng2TreeSettings;
  constructor(private _translate: TranslateService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
    private questionTagsService: QuestionTagsService,
    private questionService: QuestionsService,
    private utilitiesService: UtilitiesService,
    private rd: Renderer,
    private zone: NgZone) {
    this.registerCKEditorCommands = this.registerCKEditorCommands.bind(this);
  }

  ngOnInit() {
    let self = this;
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
      this.question.month = new Date();
    }

    this.getQuestionTags();
    this.buildQuestionTypes();
    this.treeSetting = {
      rootIsVisible: false
    };
  };

  buildQuestionTypes() {
    this.questionTypes.push({
      id: QuestionTypes.SingleChoice,
      name: this._translate.instant('ENUM_QUESTION_TYPES_SINGLE_CHOICE')
    }, {
        id: QuestionTypes.MultipleChoice,
        name: this._translate.instant('ENUM_QUESTION_TYPES_MULTIPLE_CHOICES')
      }
    );
  };

  registerCKEditorCommands(editor: any) {
    let self = this;
    editor.addCommand("image", {
      exec: self.onUploadImage.bind(this)
    });
  };

  ngAfterViewInit() {
    let self = this;
    for (var instanceName in CKEDITOR.instances) {
      self.editor = CKEDITOR.instances[instanceName];
      self.editor.on("instanceReady", function (ev: any) {
        let _editor = ev.editor;
        self.registerCKEditorCommands(_editor);
      });
    }
  }

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
    },
      error => {
        self.notificationService.printErrorMessage('Failed to load question. ' + error);
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

    if (!this.question.answers.find(x => x.dss == true)) {
      return false;
    }

    return true;
  };

  getQuestionTags = () => {
    var self = this;
    self.questionTagsService.get(1)
      .subscribe((questionTags) => {
        self.questionTags = questionTags;
        self.buildTree(self.questionTags);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  };

  buildTree(questionTags: QuestionTag[]) {
    let self = this;
    self.tree = {
      value: self._translate.instant('QUESTION_GROUP'),
      id: 0,
      children: []
    };

    questionTags.forEach(qtag => {
      let node: ESTreeNode = {
        id: qtag.id,
        value: qtag.name
      };

      self.buildSubTree(node);

      self.tree.children.push(node);
    });
  };

  buildSubTree(node: ESTreeNode) {
    let self =this;
    node.loadChildren = (callback) => {
      self.questionTagsService.getById(node.id)
        .subscribe((_qtag: QuestionTag) => {
          let children: ESTreeNode[] = [];
          if (_qtag.subQTags && _qtag.subQTags.length > 0) {
            _qtag.subQTags.forEach(qtag => {
              let childNode: ESTreeNode = {
                id: qtag.id,
                value: qtag.name
              };
              self.buildSubTree(childNode);
              children.push(childNode);
            });
          }
          callback(children);
        },
        error => {
          self.notificationService.printErrorMessage('Failed to load question tag. ' + error);
        });
    };
  };

  cancel(): void { this.router.navigate(['/admin/questions']); };

  save(): void {
    let self = this, promise = null;

    if (self.questionId) {
      promise = self.questionService.update(self.question);
    } else {
      promise = self.questionService.create(self.question);
    }

    promise.subscribe((id: any) => {
      if (!self.questionId) {
        self.question.id = id;
      }

      self.alert.type = 'success';
      self.alert.message = self._translate.instant('SAVED');
      this.router.navigate(['/admin/questions']);
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

  onClickRating(event: any) {
    console.log('onClickRating: ' + event);
  };

  onRatingChange(event: any) {
    console.log('onRatingChange: ' + event);
  };

  onUploadImage(editor: any) {
    this.uploadModal.show();
  };

  onFileChange(event: any) {
    let self = this;
    if (event.target.files && event.target.files.length > 0) {
      let fileToUpload = event.target.files[0];
      self.file.name = fileToUpload.name;
      self.file.type = fileToUpload.type;
      self.file.size = fileToUpload.size;
    }
  };

  uploadFile() {
    let fi = this.fileInput.nativeElement, self = this;
    if (fi.files && fi.files[0]) {
      let fileToUpload = fi.files[0];
      this.questionService
        .upload(fileToUpload)
        .subscribe((response) => {
          self.file.id = response.id;
          self.file.content = response.content;
        },
        error => {
          self.notificationService.printErrorMessage('Failed to upload file. ' + error);
        });
    }
  };

  onUploaded(): void {
    let self = this;
    let imageElement = String.format('<img alt="{0}" title="{0}" src="{1}" />', self.file.name, self.questionService.getUploadFileUrl() + '/' + self.file.id);
    this.editor.insertHtml(imageElement);
    this.uploadModal.hide();
  };

  dateModelChange(dt: Date) {
    this.question.month = dt;
  };

  handleSelected(event: any): void {
    this.question.qtagId = event.node.node.id;
  };
}