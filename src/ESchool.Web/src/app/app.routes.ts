import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
//import { QuestionsComponent } from './questions/questions.component';
import { QuestionTagsComponent } from './questionTags/question-tags.component';
import { QuestionPapersComponent } from './questionPapers/question-papers.component';
import { PageNotFoundComponent } from './errors/page-not-found.component';

export const ROUTES: Routes = [
  { path: '', component: HomeComponent },
  { path: 'questionTags', component: QuestionTagsComponent },
  {
    path: 'questions',
    loadChildren: 'app/questions/questions.module#QuestionsModule',
    //component: QuestionsComponent
    //canLoad: [AuthGuard]
  },
  { path: 'questionPapers', component: QuestionPapersComponent },
  { path: '**', component: PageNotFoundComponent }
];
