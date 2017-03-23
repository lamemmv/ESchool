import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { SchoolHomeComponent } from '../components/school-app.component';
import { StudentListComponent } from '../components/students/student-list.component';

export { SchoolHomeComponent };

@NgModule({
  bootstrap: [SchoolHomeComponent],
  declarations: [SchoolHomeComponent, StudentListComponent],
  imports: [BrowserModule],
  providers: []
})
export class MainModule {}
