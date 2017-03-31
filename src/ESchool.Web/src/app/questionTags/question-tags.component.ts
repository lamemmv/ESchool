import { Component, OnInit } from '@angular/core';
import { CommonModule }   from '@angular/common';
import { FormsModule }   from '@angular/forms';

import { NotificationService } from './../shared/utils/notification.service';
import { QuestionTag } from './question-tags.model';
import { QuestionTagsService } from './question-tags.service'

@Component({
  selector: 'question-tags',
  templateUrl: './question-tags.component.html',
})
export class QuestionTagsComponent implements OnInit  {
    private questionTag: QuestionTag;
    private questionTags: QuestionTag[];
    constructor(private questionTagsService: QuestionTagsService,
      private notificationService: NotificationService){

    }

    ngOnInit() {
      this.questionTag = new QuestionTag();
      this.questionTags = [];
      this.getQuestionTags();
    };

    getQuestionTags=()=>{
      var self = this;
      self.questionTagsService.get()
        .subscribe((questionTags) => {
              self.questionTags = questionTags;
          }, 
          error => {
              self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
          });
    };

    addQuestionTag=()=>{
      var self = this;
      self.questionTagsService.create(self.questionTag)
        .subscribe((questionTagCreated) => {
              self.questionTag = questionTagCreated;
          }, 
          error => {
              self.notificationService.printErrorMessage('Failed to create question tag. ' + error);
          });
    };
  }