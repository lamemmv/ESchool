import { Component, OnInit } from '@angular/core';

import { PagedList } from './../exam-papers.model';
@Component({
    selector: 'exam-paper-list',
    templateUrl: './exam-paper-list.component.html',
    styleUrls: [
        ('./exam-paper-list.style.scss'),
    ],
})

export class ExamPaperListComponent implements OnInit {
    private examPaper = new PagedList();
    constructor() { }

    ngOnInit() {

    }

    onPageChange(page: number) {

    }
}
