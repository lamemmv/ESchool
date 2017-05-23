import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotificationService } from './../../../../../shared/utils/notification.service';
import { PagedList, ExamPaper } from './../exam-papers.model';
import { ExamPapersService } from './../exam-papers.service';
import { EditExamPaperComponent } from './../edit/exam-paper-edit.component';
import { ConfigService } from './../../../../../shared/utils/config.service';
@Component({
    selector: 'exam-paper-list',
    templateUrl: './exam-paper-list.component.html',
    styleUrls: [
        ('./exam-paper-list.style.scss'),
    ],
})

export class ExamPaperListComponent implements OnInit {
    private examPaper = new PagedList();
    constructor(private router: Router,
        private examPapersService: ExamPapersService,
        configService: ConfigService,
        private notificationService: NotificationService) {
        this.examPaper.page = 1;
        this.examPaper.size = configService.getPageSize();
    }
    ngOnInit() {
        this.getExamPapers();
    }

    getExamPapers() {
        const self = this;
        this.examPapersService.get(self.examPaper.page, self.examPaper.size).subscribe((response: PagedList) => {
            this.examPaper = response;
        },
        error => {
            self.notificationService.printErrorMessage('Failed to load question tags. ' + error);
        });
    }

    onPageChange(page: number) {

    }

    add() {
        this.router.navigate(['/pages/components/examPapers/create']);
    }
    
    handleDialogClose(result: any) {

    }
}
