import { Component, OnInit, HostListener } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { TreeNode } from 'primeng/primeng';

import { NotificationService } from './../../../../../shared/utils/notification.service';
import { QuestionTag } from './../../questionTags/question-tags.models';
import { QuestionTagsService } from './../../questionTags/question-tags.service';
import { ExamPaperPart } from './../exam-papers.model';
@Component({
  selector: 'exam-paper-part',
  styleUrls: [('./exam-paper-part.style.scss')],
  templateUrl: './exam-paper-part.component.html'
})

export class ExamPaperPartComponent implements OnInit {

  modalHeader: string;
  modalContent: any;
  private selectedQTag = new QuestionTag();
  private questionTags: QuestionTag[] = [];
  private dataGrid: TreeNode[];
  private showTree: boolean = false;
  private part = new ExamPaperPart();

  constructor(private activeModal: NgbActiveModal,
    private questionTagsService: QuestionTagsService,
    private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.getQuestionTags();
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

  handleSelected(event: any): void {
    let self = this;
    this.part.qTagId = event.node.data.id;
    self.questionTagsService.getById(event.node.data.id)
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

  @HostListener('click', ['$event'])
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
                data: subTag, children: children, leaf: subTag.subQTagsCount > 0 ? false : true
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

  save() {
    this.part.qTagPath = this.selectedQTag.path;
    this.activeModal.close(this.part);
  }

  cancel() {
    this.activeModal.dismiss();
  }

}
