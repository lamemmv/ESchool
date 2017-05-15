import { Component, OnInit } from '@angular/core';
import { AlertModule } from 'ng2-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'edit-question',
  templateUrl: './question-edit.component.html',
  styleUrls: [
    './question-edit.style.css'
  ]
})

export class EditQuestionComponent implements OnInit {
  constructor(private _translate: TranslateService) {
  }

  ngOnInit() {
    
  }
}
