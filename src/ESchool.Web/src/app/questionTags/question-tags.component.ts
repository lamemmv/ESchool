import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AlertModule } from 'ng2-bootstrap';
import { ModalDirective } from 'ng2-bootstrap';
import { DialogService } from "ng2-bootstrap-modal";
import { TreeNode } from 'primeng/primeng';

import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';

// Observable class extensions
import 'rxjs/add/observable/of';

// Observable operators
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/distinctUntilChanged';

import { NotificationService } from './../shared/utils/notification.service';
import { TranslateService } from './../shared/translate';
import { ConfirmDialogComponent } from './../shared/modals/confirm-dialog.component';
import { Modal } from './../shared/modals/models';

import { QuestionTag } from './question-tags.model';
import { AlertModel } from './../shared/models/alerts';
import { QuestionTagsService } from './question-tags.service';
import { GroupsService } from './../groups/groups.service';
import { Group } from './../groups/groups.model';

@Component({
  selector: 'question-tags',
  templateUrl: './question-tags.component.html',
  styleUrls: [
    './question-tags.style.css',
    './../../plugins/datatables/dataTables.bootstrap.css'
  ],
  encapsulation: ViewEncapsulation.None
})
export class QuestionTagsComponent implements OnInit {
  @ViewChild('childModal')
  public childModal: ModalDirective;
  private questionTag: QuestionTag;
  private questionTags: QuestionTag[];
  private originalQTags: QuestionTag[];
  private alert: AlertModel;
  private DESCRIPTION: string;
  private searchTerms = new Subject<string>();
  filteredList = new Observable<QuestionTag[]>();
  private modal = new Modal();
  private groups: Group[];
  private selectedGroup = new Group();
  private dataGrid: TreeNode[];
  constructor(private questionTagsService: QuestionTagsService,
    private groupService: GroupsService,
    private notificationService: NotificationService,
    private _translate: TranslateService,
    private dialogService: DialogService) {
  }

  ngOnInit() {
    this.questionTag = new QuestionTag();
    this.questionTags = [];
    this.originalQTags = [];
    this.groups = [];
    this.dataGrid = [];
    this.alert = {
      type: '',
      message: ''
    };
    this.modal.cancelText = this._translate.instant('BUTTON_CANCEL');
    this.DESCRIPTION = this._translate.instant('DESCRIPTION');

    this.filteredList = this.searchTerms
      .debounceTime(1000)        // wait 300ms after each keystroke before considering the term
      .distinctUntilChanged()   // ignore if next search term is same as previous
      .switchMap(term => term   // switch to new observable each time the term changes
        // return the http search observable
        ? this.search(term)
        // or the observable of empty heroes if there was no search term
        : Observable.of<QuestionTag[]>([]))
      .catch(error => {
        // TODO: add real error handling
        console.log(error);
        return Observable.of<QuestionTag[]>([]);
      });

    this.getGroups();
  };

  getGroups() {
    let self = this;
    self.groupService.get()
      .subscribe((groups) => {
        self.groups = groups;
        self.selectedGroup = self.groups[0];
        self.getQuestionTags(self.selectedGroup.id);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  };

  onChangeGroup(group: Group) {
    this.getQuestionTags(this.selectedGroup.id);
    this.questionTag.groupId = this.selectedGroup.id;
  };

  getQuestionTags = (groupId: number) => {
    var self = this;
    self.questionTagsService.get(groupId)
      .subscribe((questionTags) => {
        self.questionTags = questionTags;
        self.originalQTags = self.cloneArray(self.questionTags);
        self.dataGrid = self.getDataGrid(self.questionTags);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  };

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
  };

  addQuestionTag = () => {
    var self = this;
    self.questionTagsService.create(self.questionTag)
      .subscribe((id: number) => {
        self.questionTag.id = id;
        self.alert.type = 'success';
        self.alert.message = this._translate.instant('SAVED');
        self.getQuestionTags(self.selectedGroup.id);
        this.childModal.hide();
      },
      (error) => {
        self.alert.type = 'danger';
        self.alert.message = self._translate.instant(JSON.parse(error._body).code);
        this.childModal.hide();
      });
  };

  removeQTag = (event: any, qtag: QuestionTag) => {
    event.stopPropagation();
    var self = this;
    this.dialogService.addDialog(ConfirmDialogComponent, {
      title: this._translate.instant('QUESTION_TAGS'),
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
              self.getQuestionTags(self.selectedGroup.id);
            },
            error => {
              self.notificationService.printErrorMessage('Failed to delete question tag. ' + error);
            });
        }
      });
  };

  updateQuestionTag = () => {
    var self = this;
    self.questionTagsService.update(self.questionTag)
      .subscribe((questionTagCreated) => {
        self.alert.type = 'success';
        self.alert.message = this._translate.instant('SAVED');
        self.getQuestionTags(self.selectedGroup.id);
        self.childModal.hide();
        self.questionTag = new QuestionTag();
      },
      error => {
        self.notificationService.printErrorMessage('Failed to update question tag. ' + error);
      });
  };

  submitForm = () => {
    if (this.questionTag.id) {
      this.updateQuestionTag();
    } else {
      this.addQuestionTag();
    }
  };

  openAddDialog() {
    this.modal.okText = this._translate.instant('BUTTON_SAVE');
    this.modal.title = this._translate.instant('ADD_QUESTION_TAG_TITLE');
    this.showChildModal(null);
  };

  openEditDialog(event: any) {
    this.modal.okText = this._translate.instant('UPDATE');
    this.modal.title = this._translate.instant('EDIT_QUESTION_TAG_TITLE');
    this.showChildModal(event.node.data);
  };

  showChildModal(qtag: QuestionTag): void {
    this.questionTag = Object.assign({}, qtag);
    this.questionTag.groupId = this.selectedGroup.id;
    this.childModal.show();
  };

  cancelUpdate = () => {
    this.childModal.hide();
  };

  search(term: string): Observable<QuestionTag[]> {
    var self = this;
    let filtered: QuestionTag[] = [];
    console.log(term);

    if (!term.trim()) {
      self.questionTags = self.cloneArray(self.originalQTags);
      self.dataGrid = self.getDataGrid(self.questionTags);
      return Observable.of(self.originalQTags);
    } else {
      let items = this.originalQTags.filter(qtag => qtag.name.toLowerCase().indexOf(term.toLowerCase()) > -1);
      self.questionTags = self.cloneArray(items);
      self.dataGrid = self.getDataGrid(self.questionTags);
      return Observable.of(items);
    }
  };

  onKeyStroke(keyword: string): void {
    this.searchTerms.next(keyword);
    if (!keyword.trim()) {
      this.questionTags = this.cloneArray(this.originalQTags);
      this.dataGrid = this.getDataGrid(this.questionTags);
    }
  };

  cloneArray(src: any[]) {
    let arr: any[] = [];
    src.forEach((x) => {
      arr.push(Object.assign({}, x));
    });
    return arr;
  };

  onNodeExpand(e: any) {
    let self = this;
    if (e.node) {
      self.questionTagsService.getById(e.node.data.id)
        .subscribe((qtag: QuestionTag) => {
          let nodes : TreeNode[] = [];
          if (qtag.subQTags) {
            qtag.subQTags.forEach((subTag) => {
              let children : any[] = [];
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
    }
  };
}