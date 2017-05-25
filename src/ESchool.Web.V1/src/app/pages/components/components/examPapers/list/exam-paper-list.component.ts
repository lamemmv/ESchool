import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from './../../../../../shared/utils/notification.service';
import { PagedList, ExamPaper } from './../exam-papers.model';
import { ExamPapersService } from './../exam-papers.service';
import { ConfirmDeleteExamPaperComponent } from './../delete/exam-paper-delete.component';
import { EditExamPaperComponent } from './../edit/exam-paper-edit.component';
import { ConfigService } from './../../../../../shared/utils/config.service';
@Component({
    selector: 'nga-exam-paper-list',
    templateUrl: './exam-paper-list.component.html',
    styleUrls: [
        ('./exam-paper-list.style.scss'),
    ],
})

export class ExamPaperListComponent implements OnInit {
    private examPaper = new PagedList();
    constructor(private router: Router,
        private examPapersService: ExamPapersService,
        private configService: ConfigService,
        private notificationService: NotificationService,
        private modalService: NgbModal) {
        this.examPaper.page = 1;
        this.examPaper.size = configService.getPageSize();
    }

    ngOnInit() {
        this.getExamPapers(this.examPaper.page);
    }

    getExamPapers(page) {
        const self = this;
        this.examPapersService.get(page, self.examPaper.size).subscribe((response: PagedList) => {
            this.examPaper = response;
        },
            error => {
                self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
            });
    }

    onPageChange(page: number) {
        this.getExamPapers(page);
    }

    add() {
        this.router.navigate(['/pages/components/examPapers/create']);
    }

    editExamPaper(examPaper: ExamPaper) {
        this.router.navigate(['/pages/components/examPapers/edit', examPaper.id]);
    }

    removeExamPaper(ep: ExamPaper) {
        this.openDialog(ep);
    }

    openDialog(content: ExamPaper) {
        const self = this;
        const activeModal = this.modalService.open(ConfirmDeleteExamPaperComponent, {
            size: 'sm',
            backdrop: 'static',
        });
        activeModal.componentInstance.modalContent = content;
        activeModal.result.then((result) => {
            self.handleDialogClose(result);
        }, (reason) => {
            self.handleDialogClose(null);
        });
    }

    handleDialogClose(result: any) {
        if (result) {
            this.getExamPapers(this.examPaper.page);
        }
    }
}
