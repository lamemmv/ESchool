import { NgModule }              from '@angular/core';
import { RouterModule, Routes }  from '@angular/router';
import { HomeComponent } from './home/home.component'; 
import { QuestionTagsComponent } from './questionTags/question-tags.component';
//import { QuestionPapersComponent } from './questionPapers/question-papers.component';
import { PageNotFoundComponent } from './errors/page-not-found.component';
const appRoutes: Routes = [
  { path: 'home',   component: HomeComponent },
  { path: 'questionTags', component: QuestionTagsComponent },
  //{ path: 'questionPapers', component: QuestionPapersComponent },
  { path: '',   redirectTo: '/home', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }
];
@NgModule({
  imports: [
    RouterModule.forRoot(appRoutes)
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {}
