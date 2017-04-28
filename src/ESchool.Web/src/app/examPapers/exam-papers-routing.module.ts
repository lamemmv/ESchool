import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExamPapersComponent } from './exam-papers.component';

const examPapersRoutes: Routes = [
    {
        path: 'examPapers',
        component: ExamPapersComponent,
        children: [
            
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(examPapersRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class ExamPapersRoutingModule { }
