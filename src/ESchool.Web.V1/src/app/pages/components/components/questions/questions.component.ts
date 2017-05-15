import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { TreeModel } from 'ng2-tree';
import { AlertModule } from 'ng2-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { TreeNode } from 'primeng/primeng';
import { DialogService } from "ng2-bootstrap-modal";
import { Observable } from 'rxjs/Observable';
import { Subject } from 'rxjs/Subject';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { NotificationService } from './../../../../shared/utils/notification.service';
import { GroupsService } from './../groups/groups.service';
import { Group } from './../groups/groups.model';
import { AlertModel } from './../../../../shared/models/alert';
import { ConfirmDialogComponent } from './../../../../shared/modals/confirm-dialog.component';
import { Modal } from './../../../../shared/models/modal';

@Component({
  selector: 'questions',
  templateUrl: './questions.component.html',
  styleUrls: [
    './questions.style.css'
  ],
  encapsulation: ViewEncapsulation.None
})

export class QuestionsComponent implements OnInit {
  constructor(private _translate: TranslateService) {
  }

  ngOnInit() {
    
  }
}
