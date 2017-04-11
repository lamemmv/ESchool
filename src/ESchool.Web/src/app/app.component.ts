import { Component } from '@angular/core';

import { TranslateService } from './shared/translate';
import { BreadcrumbService } from 'ng2-breadcrumb/ng2-breadcrumb';

@Component({
  selector: 'eschool-app',
  templateUrl: './app.component.html',
  styleUrls: [
    "./app.style.css"
  ]
})
export class AppComponent {
  constructor(private _translate: TranslateService,
    private _breadcrumb: BreadcrumbService) {
    this._translate.use('vi');
    _breadcrumb.addFriendlyNameForRoute('/', 'ESchool');
    _breadcrumb.addFriendlyNameForRoute('/questionTags', this._translate.instant('QUESTION_TAGS'));
    _breadcrumb.addFriendlyNameForRouteRegex('/questions', this._translate.instant('QUESTIONS'));
  }
}
