import { Component, OnInit } from '@angular/core';

import { OidcSecurityService } from './security/auth';

@Component({
  selector: 'qms-app',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private securityService: OidcSecurityService) {

  }

  ngOnInit() {
    // if (window.location.hash) {
    //   this.securityService.authorizedCallback();
    // }
  }
}
