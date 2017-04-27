import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminComponent } from './admin.component';
import { QuestionsComponent } from './../questions/questions.component';
import { QuestionTagsComponent } from './../questionTags/question-tags.component';
import { AdminHomeComponent } from './home/admin-home.component';

const adminRoutes: Routes = [
    {
        path: 'admin',
        component: AdminComponent,
        children: [
            {
                path: '',
                component: AdminHomeComponent,
                children: [
                    {
                        path: 'questions',
                        component: QuestionsComponent
                    },
                    {
                        path: 'questionTags',
                        component: QuestionTagsComponent
                    }
                ]
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
