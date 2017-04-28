import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { AdminHomeComponent } from './home/admin-home.component';
import { QuestionTagsComponent } from './../questionTags/question-tags.component';
import { AuthGuard } from './../shared/authentications/auth-guard.service';

const adminRoutes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        children: [
            {
                path: '',
                component: AdminHomeComponent
            },
            {
                path: 'questions',
                loadChildren: 'app/questions/questions.module#QuestionsModule',
                data: { preload: true }
            },
            {
                path: 'questionTags',
                component: QuestionTagsComponent
            },
            {
                path: 'examPapers',
                loadChildren: 'app/examPapers/exam-papers.module#ExamPapersModule',
                canLoad: [AuthGuard]
            }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(adminRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class AdminRoutingModule { }
