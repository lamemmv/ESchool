import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Router } from '@angular/router';

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
    examPaper = new ExamPaper();
    groups: Group[] = [];
    selectedGroup = new Group();
    constructor(private router: Router,
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

                if (this.examPaper.id) {
                    this.view.title = this.examPaperTranslation.editExamPaper;
                    this.view.okText = this.examPaperTranslation.updateText;
                } else {
                    this.view.title = this.examPaperTranslation.addExamPaper;
                    this.view.okText = this.examPaperTranslation.saveText;
                    this.examPaper = new ExamPaper();
                    this.examPaper.fromDate = new Date();
                    this.examPaper.toDate = new Date();
                }
            });
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
        this.router.navigate(['/pages/components/examPapers']);
    }

    save(): void {
        let self = this, observable = null;

        if (self.examPaper.id) {
            observable = self.examPaperService.update(self.examPaper);
        } else {
            observable = self.examPaperService.create(self.examPaper);
        }

        observable.subscribe((id: any) => {
            if (!self.examPaper.id) {
                self.examPaper.id = id;
            }

            this.router.navigate(['/pages/components/examPapers']);
        },
            error => {
                self.notificationService.printErrorMessage('Failed to create question. ' + error);
            });
    }

    isValid(): boolean {
        if (!this.examPaper.name) {
            return false;
        }

        if (this.examPaper.parts.length == 0) {
            return false;
        }

        if (this.examPaper.duration <= 0) {
            return false;
        }

        return true;
    }

    addPart() {

    }
}
