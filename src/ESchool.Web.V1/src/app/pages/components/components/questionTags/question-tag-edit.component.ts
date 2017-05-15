import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TreeNode } from 'primeng/primeng';

import { QuestionTag } from './question-tags.models';
import { QuestionTagsService } from './question-tags.service';
import { NotificationService } from './../../../../shared/utils/notification.service';
@Component({
  selector: 'edit-question-tag',
  styleUrls: [('./question-tag-edit.style.scss')],
  templateUrl: './question-tag-edit.component.html',
  host: {
    '(document:click)': 'onDocumentClick($event)',
  }
})

export class EditQuestionTagComponent implements OnInit {

  modalHeader: string;
  modalContent: QuestionTag;  
  private selectedQTag = new QuestionTag();
  private showTree: boolean = false;
  private dataGrid: TreeNode[];

  constructor(private activeModal: NgbActiveModal,
    private questionTagsService: QuestionTagsService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.getQuestionTags();
    if (this.modalContent.id && this.modalContent.parentQTags){
      this.selectedQTag.path = this.getPath(this.modalContent);
    }
  }

  save() {
    if (this.modalContent.id){
      this.updateQuestionTag();
    }else{
      this.addQuestionTag();
    }
  }

  cancel() {
    this.activeModal.dismiss();
  }

  addQuestionTag = () => {
    var self = this;
    self.questionTagsService.create(self.modalContent)
      .subscribe((id: number) => {
        self.modalContent.id = id;
        self.activeModal.close(self.modalContent);
      },
      (error) => {
        self.activeModal.dismiss();
      });
  }

  updateQuestionTag = () => {
    var self = this;
    self.questionTagsService.update(self.modalContent)
      .subscribe((questionTagCreated) => {
        self.activeModal.close(self.modalContent);
      },
      error => {
        self.activeModal.dismiss();
      });
  }

  handleSelected(event: any): void {
    let self = this;
    this.modalContent.parentId = event.node.data.id;
    self.questionTagsService.getById(this.modalContent.parentId)
      .subscribe((qtag: QuestionTag) => {
        this.selectedQTag = qtag;
        this.selectedQTag.path = this.getPath(this.selectedQTag) + this.selectedQTag.name;
      },
      error => {
        self.notificationService.printErrorMessage('Failed to update question tag. ' + error);
      });
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

  getQuestionTags = () => {
    var self = this;
    self.questionTagsService.get(1)
      .subscribe((questionTags) => {
        self.dataGrid = self.getDataGrid(questionTags);
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
}
