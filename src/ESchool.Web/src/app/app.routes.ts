import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { QuestionTagsComponent } from './questionTags/question-tags.component';
import { QuestionsComponent } from './questions/questions.component';
import { QuestionPapersComponent } from './questionPapers/question-papers.component';

export const ROUTES: Routes = [
  { path: '',               component: HomeComponent },
  { path: 'questionTags',   component: QuestionTagsComponent },
  { path: 'questions',      component: QuestionsComponent },
  { path: 'questionPapers',      component: QuestionPapersComponent }
];
