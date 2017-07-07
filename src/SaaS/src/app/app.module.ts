import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { routing } from './app.routing.module';
import { PagesModule } from './pages';
import { AppTranslationModule } from './translation.module';
import { AuthService, AuthGuard } from './security';
import { AppService } from './shared/app.service';
import { ConfigService, OpenIDConfiguration } from './shared/utils/config.service';
import { AuthModule, OidcSecurityService } from './security/auth';
@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    HttpModule,
    BrowserModule,
    RouterModule,
    AuthModule.forRoot(),
    AppTranslationModule,
    PagesModule,
    routing
  ],
  providers: [
    ConfigService,
    AppService,
    AuthService,
    AuthGuard
  ],
  bootstrap: [
    AppComponent
  ]
})
export class AppModule {
  constructor(private oidcSecurityService: OidcSecurityService) {
    let openIDConfiguration = new OpenIDConfiguration();

    openIDConfiguration.redirectUrl = 'https://localhost:53090';
    // The Client MUST validate that the aud (audience) Claim contains its client_id value registered at the Issuer identified by the iss (issuer) Claim as an audience.
    // The ID Token MUST be rejected if the ID Token does not list the Client as a valid audience, or if it contains additional audiences not trusted by the Client.
    openIDConfiguration.clientId = 'angularclient';
    openIDConfiguration.responseType = 'id_token token';
    openIDConfiguration.scope = 'dataEventRecords securedFiles openid';
    openIDConfiguration.postLogoutRedirectUri = 'https://localhost:53090/unauthorized';
    openIDConfiguration.startCheckSession = true;
    openIDConfiguration.silentRenew = true;
    openIDConfiguration.startupRoute = '/dataeventrecords';
    // HTTP 403
    openIDConfiguration.forbiddenRoute = '/forbidden';
    // HTTP 401
    openIDConfiguration.unauthorizedRoute = '/unauthorized';
    openIDConfiguration.logConsoleWarningActive = true;
    openIDConfiguration.logConsoleDebugActive = false;
    // id_token C8: The iat Claim can be used to reject tokens that were issued too far away from the current time,
    // limiting the amount of time that nonces need to be stored to prevent attacks.The acceptable range is Client specific.
    openIDConfiguration.maxIdTokenIatOffsetAllowedInSeconds = 10;

    // this.oidcSecurityService.setStorage(localStorage);
    this.oidcSecurityService.setupModule(openIDConfiguration);
  }
}
