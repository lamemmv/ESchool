import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';

import { NotificationService } from './../../../../../shared/utils/notification.service';
import { ExamPaper, ModalView } from './../exam-papers.model';
import { ExamPapersService } from './../exam-papers.service';
import { Group } from './../../groups/groups.model';
import { GroupsService } from './../../groups/groups.service';
@Component({
    selector: 'exam-paper-edit',
    templateUrl: './exam-paper-edit.component.html',
    styleUrls: [
        ('./exam-paper-edit.style.scss'),
    ],
})

export class EditExamPaperComponent implements OnInit {
    view = new ModalView();
    examPaperTranslation = {
        editExamPaper: '',
        addExamPaper: '',
        saveText: '',
        updateText: '',
    };
    modalContent: ExamPaper;
    groups: Group[] = [];
    selectedGroup = new Group();
    constructor(private activeModal: NgbActiveModal,
        private translate: TranslateService,
        private groupService: GroupsService,
        private notificationService: NotificationService,
        private examPaperService: ExamPapersService) { }

    ngOnInit() {
        this.translate.get(['EDIT_EXAM_PAPER',
            'ADD_EXAM_PAPER', 'UPDATE', 'SAVE']).subscribe((res: any) => {
                this.examPaperTranslation.editExamPaper = res.EDIT_EXAM_PAPER;
                this.examPaperTranslation.addExamPaper = res.ADD_EXAM_PAPER;
                this.examPaperTranslation.updateText = res.UPDATE;
                this.examPaperTranslation.saveText = res.SAVE;
            });
        if (this.modalContent.id) {
            this.view.title = this.examPaperTranslation.editExamPaper;
            this.view.okText = this.examPaperTranslation.updateText;
        } else {
            this.view.title = this.examPaperTranslation.addExamPaper;
            this.view.okText = this.examPaperTranslation.saveText;
            this.modalContent = new ExamPaper();
            this.modalContent.fromDate = new Date();
            this.modalContent.toDate = new Date();
        }

        this.getGroups();
    }

    getGroups() {
        const self = this;
        self.groupService.get()
            .subscribe((groups) => {
                self.groups = groups;
                self.selectedGroup = self.groups[0];
            },
            error => {
                self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
            });
    }

    cancel() {
        this.activeModal.dismiss();
    }

    save(): void {
        let self = this, promise = null;

        if (self.modalContent.id) {
            promise = self.examPaperService.update(self.modalContent);
        } else {
            promise = self.examPaperService.create(self.modalContent);
        }

        promise.subscribe((id: any) => {
            if (!self.modalContent.id) {
                self.modalContent.id = id;
            }

            self.activeModal.close(self.modalContent);
        },
        error => {
            self.notificationService.printErrorMessage('Failed to create question. ' + error);
        });
    }
}
