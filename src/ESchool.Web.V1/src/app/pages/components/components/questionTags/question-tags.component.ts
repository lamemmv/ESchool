import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TreeModel } from 'ng2-tree';
import { AlertModule } from 'ng2-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { TreeNode } from 'primeng/primeng';
import { DialogService } from "ng2-bootstrap-modal";
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { NotificationService } from './../../../../shared/utils/notification.service';
import { QuestionTagsService } from './question-tags.service';
import { GroupsService } from './../groups/groups.service';
import { Group } from './../groups/groups.model';
import { AlertModel } from './../../../../shared/models/alert';
import { ConfirmDialogComponent } from './../../../../shared/modals/confirm-dialog.component';
import { Modal } from './../../../../shared/models/modal';
import { QuestionTag } from './question-tags.models';
import { EditQuestionTagComponent } from './question-tag-edit.component';

@Component({
  selector: 'question-tags',
  templateUrl: './question-tags.component.html',
  styleUrls: [
    './question-tags.style.css'
  ],
  encapsulation: ViewEncapsulation.None
})

export class QuestionTagsComponent implements OnInit {
  private alert: AlertModel;
  private questionTag = new QuestionTag();
  private questionTags: QuestionTag[] = [];
  private originalQTags: QuestionTag[] = [];
  private modal = new Modal();
  private questionTagsTranslation = {
    searchPlaceholder: ''
  };
  private groups: Group[] = [];
  private selectedGroup = new Group();
  private dataGrid: TreeNode[] = [];
  private searchTerms = new Subject<string>();
  private filteredList = new Observable<QuestionTag[]>();
  constructor(private _translate: TranslateService,
    private groupService: GroupsService,
    private questionTagsService: QuestionTagsService,
    private notificationService: NotificationService,
    private dialogService: DialogService,
    private modalService: NgbModal) {
  }

  ngOnInit() {
    this.alert = {
      type: '',
      message: ''
    };

    this._translate.get('PLACEHOLDER_SEARCH_QUESTION_TAGS').subscribe((res: string) => {
      this.questionTagsTranslation.searchPlaceholder = res;
    });

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
      },
      (error) => {
        self.alert.type = 'danger';
        self.alert.message = self._translate.instant(JSON.parse(error._body).code);
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
    const activeModal = this.modalService.open(EditQuestionTagComponent, {size: 'sm',
                                                              backdrop: 'static'});
    activeModal.componentInstance.modalHeader = 'Static modal';
    activeModal.componentInstance.modalContent = `This is static modal, backdrop click
                                                    will not close it. Click × or confirmation button to close modal.`;
  };

  openEditDialog(event: any) {
    this.showChildModal(event.node.data);
  };

  showChildModal(qtag: QuestionTag): void {
    this.questionTag = Object.assign({}, qtag);
    this.questionTag.groupId = this.selectedGroup.id;
  };

  cancelUpdate = () => {
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
            qtag.subQTags.forEach((subTag: QuestionTag) => {
              let children : any[] = [];
              let treeNode = {
                data: subTag, children: children, leaf: subTag.subQTagsCount > 0 ? false: true
              };
              nodes.push(treeNode);
            });
          }

          e.node.children = nodes;
        },
        error => {
          self.notificationService.printErrorMessage('Failed to load question tag. ' + error);
        });
    }
  };
}
