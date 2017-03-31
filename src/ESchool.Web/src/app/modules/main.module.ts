import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
//import { FormsModule } from '@angular/forms';

import { SchoolHomeComponent } from '../components/school-app.component';
import { StudentListComponent } from '../components/students/student-list.component';
import { QuestionTagsComponent } from '../components/questionTags/question-tags.component';
import { QuestionsComponent } from '../components/questions/questions.component';

// import { SlimLoadingBarService, SlimLoadingBarComponent } from 'ng2-slim-loading-bar';

import { routing } from './app.routes';
export { SchoolHomeComponent };

@NgModule({
  bootstrap: [SchoolHomeComponent],
  declarations: [//SlimLoadingBarComponent, 
    SchoolHomeComponent, 
    StudentListComponent, 
    QuestionTagsComponent, 
    QuestionsComponent],
  imports: [BrowserModule, 
    CommonModule, 
    //FormsModule, 
    routing],
  providers: []//[SlimLoadingBarService]
})
export class MainModule {}
