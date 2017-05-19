import { Component, OnInit } from '@angular/core';

import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { PagedList, ExamPaper } from './../exam-papers.model';
import { EditExamPaperComponent } from './../edit/exam-paper-edit.component';
@Component({
    selector: 'exam-paper-list',
    templateUrl: './exam-paper-list.component.html',
    styleUrls: [
        ('./exam-paper-list.style.scss'),
    ],
})

export class ExamPaperListComponent implements OnInit {
    private examPaper = new PagedList();
    constructor(private modalService: NgbModal) { }

    ngOnInit() {

    }

    onPageChange(page: number) {

    }

    add() {
        this.openDialog(new ExamPaper());
    }

    openDialog(content: ExamPaper) {
        const self = this;
        const activeModal = this.modalService.open(EditExamPaperComponent, {
            size: 'lg',
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

    }
}
