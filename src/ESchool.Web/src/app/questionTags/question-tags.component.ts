import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AlertModule } from 'ng2-bootstrap';
import { ModalDirective } from 'ng2-bootstrap';
import { DialogService } from "ng2-bootstrap-modal";

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

import { QuestionTag } from './question-tags.model';
import { AlertModel } from './../shared/models/alerts';
import { QuestionTagsService } from './question-tags.service';

@Component({
  selector: 'question-tags',
  templateUrl: './question-tags.component.html',
  styleUrls: [
    './question-tags.style.css',
    './../../plugins/datatables/dataTables.bootstrap.css'
  ]
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

  constructor(private questionTagsService: QuestionTagsService,
    private notificationService: NotificationService,
    private _translate: TranslateService,
    private dialogService: DialogService) {
  }

  ngOnInit() {
    this.questionTag = new QuestionTag();
    this.questionTags = [];
    this.originalQTags = [];
    this.alert = {
      type: '',
      message: ''
    };
    this.getQuestionTags();
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

      
  };

  getQuestionTags = () => {
    var self = this;
    self.questionTagsService.get()
      .subscribe((questionTags) => {
        self.questionTags = questionTags;
        self.originalQTags = self.cloneArray(self.questionTags);
      },
      error => {
        self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
      });
  };

  addQuestionTag = () => {
    var self = this;
    self.questionTagsService.create(self.questionTag)
      .subscribe((id: number) => {
        self.questionTag.id = id;
        self.alert.type = 'success';
        self.alert.message = this._translate.instant('SAVED');
        self.getQuestionTags();
        this.childModal.hide();
      },
      error => {
        self.notificationService.printErrorMessage('Failed to create question tag. ' + error);
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
              self.getQuestionTags();
            },
            error => {
              self.notificationService.printErrorMessage('Failed to delete question tag. ' + error);
            });
        }
      });
  };

  openEditDialog = (qtag: QuestionTag) => {
    this.questionTag = qtag;
  };

  updateQuestionTag = () => {
    var self = this;
    self.questionTagsService.update(self.questionTag)
      .subscribe((questionTagCreated) => {
        self.alert.type = 'success';
        self.alert.message = this._translate.instant('SAVED');
        self.getQuestionTags();
        self.questionTag = new QuestionTag();
        self.childModal.hide();
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

  showChildModal(qtag: QuestionTag): void {
    this.questionTag = Object.assign({}, qtag);
    this.childModal.show();
  };

  cancelUpdate = () => {
    this.childModal.hide();
  };

  filterQuestionTags = (keyword: string) => {
    let filteredItems = [];
    if (!keyword) {
      filteredItems = this.cloneArray(this.originalQTags);
    }
    else {
      filteredItems = this.originalQTags.filter(item => item.name.indexOf(keyword) > -1);
    }

    this.questionTags = this.cloneArray(filteredItems);
  };

  search(term: string): Observable<QuestionTag[]> {
    var self = this;
    let filtered: QuestionTag[] = [];
    console.log(term);

    if (!term.trim()) {
      self.questionTags = self.cloneArray(self.originalQTags);
      return Observable.of(self.originalQTags);
    } else {
      let items = this.originalQTags.filter(qtag => qtag.name.toLowerCase().indexOf(term.toLowerCase()) > -1);
      self.questionTags = self.cloneArray(items);
      return Observable.of(items);
    }
  };

  onKeyStroke(keyword: string): void {
    this.searchTerms.next(keyword);
    if (!keyword.trim()) {
      this.questionTags = this.cloneArray(this.originalQTags);
    }
  };

  cloneArray(src: any[]) {
    let arr: any[] = [];
    src.forEach((x) => {
      arr.push(Object.assign({}, x));
    });
    return arr;
  }
}