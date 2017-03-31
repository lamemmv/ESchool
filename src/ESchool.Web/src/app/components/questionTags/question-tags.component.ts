import { Component } from '@angular/core';
//import { FormsModule } from '@angular/forms';

@Component({
  selector: 'question-tags',
  templateUrl: 'question-tags.html',
  styleUrls: ['question-tags.style.css']
})
export class QuestionTagsComponent {
  questionTag: {
    name: string,
    description: string
  };
}
