import { ModuleWithProviders }  from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SchoolHomeComponent } from '../components/school-app.component';
import { QuestionTagsComponent } from '../components/questionTags/question-tags.component';
import { QuestionsComponent } from '../components/questions/questions.component';

const appRoutes: Routes = [
    { path: 'questionTags', component: QuestionTagsComponent },
    { path: 'questions', component: QuestionsComponent },
    { path: '', component: SchoolHomeComponent }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);