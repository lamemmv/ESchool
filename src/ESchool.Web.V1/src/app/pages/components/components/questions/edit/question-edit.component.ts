import {
  Component, OnInit, ViewChild, ViewChildren, AfterViewChecked,
  AfterViewInit, Renderer, QueryList, ViewEncapsulation,
  ElementRef
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { AlertModule } from 'ng2-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TreeNode } from 'primeng/primeng';
import { RatingModule } from "ngx-rating";

import { NotificationService } from './../../../../../shared/utils/notification.service';
import { AlertModel } from './../../../../../shared/models/alert';
import { UtilitiesService } from './../../../../../shared/utils/utilities.service';
import {
  Question, QTag,
  Answer, QuestionView, QuestionType,
  QuestionTypes, FormFile
} from './../question.model';
import { QuestionTag } from './../../questionTags/question-tags.models';
import { QuestionsService } from './../questions.service';
import { QuestionTagsService } from './../../questionTags/question-tags.service';

declare var CKEDITOR: any;
@Component({
  selector: 'edit-question',
  templateUrl: './question-edit.component.html',
  styleUrls: [
    ('./question-edit.style.scss')
  ],
  host: {
    '(document:click)': 'onDocumentClick($event)',
  }
})

export class EditQuestionComponent implements OnInit, AfterViewChecked, AfterViewInit {
  @ViewChildren('answers') answerInputs: QueryList<any>;
  @ViewChild("fileInput") fileInput: any;
  private alert: AlertModel;
  private question = new Question();
  private questionTags: QuestionTag[];
  private view = new QuestionView();
  private editor: any;
  private questionTypes: QuestionType[] = new Array();
  private answerName: string = 'A';
  private hasJustAddedAnswer: boolean = false;
  private selectedQtags: QTag[] = new Array();
  private questionId: number;
  private file = new FormFile();
  private dataGrid: TreeNode[];
  private showTree: boolean = false;
  private selectedQTag = new QuestionTag();

  constructor(private _translate: TranslateService,
    private notificationService: NotificationService,
    private route: ActivatedRoute,
    private router: Router,
    private questionTagsService: QuestionTagsService,
    private questionService: QuestionsService,
    private utilitiesService: UtilitiesService,
    private rd: Renderer) {
    this.registerCKEditorCommands = this.registerCKEditorCommands.bind(this);
  }

  ngOnInit() {
    let self = this;
    this.alert = {
      type: '',
      message: '',
    };

    let id = +this.route.snapshot.params['id'];
    this.questionId = id;
    if (id) {
      this.view.title = this._translate.instant('EDIT_QUESTION_TITLE');
      this.view.okText = this._translate.instant('UPDATE');
      this.getQuestion(id);
    } else {
      this.view.title = this._translate.instant('CREATE_QUESTION_TITLE');
      this.view.okText = this._translate.instant('SAVE');
      this.question.month = new Date();
    }

    this.getQuestionTags();
    this.buildQuestionTypes();
  }

  buildQuestionTypes() {
    this._translate.get(['ENUM_QUESTION_TYPES_SINGLE_CHOICE',
      'ENUM_QUESTION_TYPES_MULTIPLE_CHOICES'])
      .subscribe((res: any) => {
        this.questionTypes.push({
          id: QuestionTypes.SingleChoice,
          name: res.ENUM_QUESTION_TYPES_SINGLE_CHOICE
        }, {
            id: QuestionTypes.MultipleChoice,
            name: res.ENUM_QUESTION_TYPES_MULTIPLE_CHOICES
          }
        );
      });
  }

  registerCKEditorCommands(editor: any) {
    let self = this;
    editor.addCommand("image", {
      exec: self.onUploadImage.bind(this)
    });
  }

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
  }

  getQuestion(id: number) {
    var self = this;
    self.questionService.getById(id).subscribe((question) => {
      self.question = question;
      self.selectedQTag = self.question.qTag;
      self.selectedQTag.path = self.getPath(self.selectedQTag) + self.selectedQTag.name;
    },
      error => {
        self.notificationService.printErrorMessage('Failed to load question. ' + error);
      });
  }

  getQuestionTags = () => {
    var self = this;
    self.questionTagsService.get(1)
      .subscribe((questionTags) => {
        self.questionTags = questionTags;
        self.dataGrid = self.getDataGrid(self.questionTags);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  }

  getDataGrid(questionTags: QuestionTag[]): TreeNode[] {
    let treeNodes: TreeNode[] = [];
    questionTags.forEach(qtag => {
      let children: TreeNode[] = [];
      let treeNode = {
        data: qtag, children: children, leaf: false
      };
      if (qtag.subQTags && qtag.subQTags.length > 0) {
        treeNode.children = this.getDataGrid(qtag.subQTags);
      }
      treeNodes.push(treeNode);
    });
    return treeNodes;
  }

  getPath(qtag: QuestionTag): string {
    let path = '';
    if (qtag.parentQTags && qtag.parentQTags.length > 0) {
      qtag.parentQTags.forEach((parent) => {
        path += parent.name + ' > ';
      });
      return path;
    }
    return '';
  }

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
  }

  onUploaded(): void {
    let self = this;
    let imageElement = String.format('<img alt="{0}" title="{0}" src="{1}" />', self.file.name, self.questionService.getUploadFileUrl() + '/' + self.file.id);
    this.editor.insertHtml(imageElement);
  }

  dateModelChange(dt: Date) {
    this.question.month = dt;
  };

  handleSelected(event: any): void {
    let self = this;
    this.question.qtagId = event.node.data.id;
    self.questionTagsService.getById(this.question.qtagId)
      .subscribe((qtag: QuestionTag) => {
        this.selectedQTag = qtag;
        this.selectedQTag.path = this.getPath(this.selectedQTag) + this.selectedQTag.name;
      },
      error => {
        self.notificationService.printErrorMessage('Failed to update question tag. ' + error);
      });
  }

  toggleTree(event: any) {
    this.showTree = !this.showTree;
    event.stopPropagation();
  }

  onDocumentClick(event: any) {
    this.showTree = false;
  }

  onNodeExpand(e: any) {
    let self = this;
    if (e.node) {
      self.questionTagsService.getById(e.node.data.id)
        .subscribe((qtag: QuestionTag) => {
          let nodes: TreeNode[] = [];
          if (qtag.subQTags) {
            qtag.subQTags.forEach((subTag) => {
              let children: any[] = [];
              let treeNode = {
                data: subTag, children: children, leaf: false
              };
              nodes.push(treeNode);
            });
          }

          e.node.children = nodes;
        },
        error => {
          self.notificationService.printErrorMessage('Failed to update question tag. ' + error);
        });
      e.originalEvent.stopPropagation();
    }
  }

  onNodeCollapse(e: any) {
    e.originalEvent.stopPropagation();
  }
}
