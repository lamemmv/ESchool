import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { routing } from './pages.routing';
import { NgaModule } from '../theme/nga.module';
import { AppTranslationModule } from '../app.translation.module';
import { AppService } from './../shared/app.service';
import { ConfigService } from './../shared/utils/config.service';
import { NotificationService } from './../shared/utils/notification.service';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';

import { Pages } from './pages.component';

@NgModule({
  imports: [CommonModule, AppTranslationModule, NgaModule, BootstrapModalModule, routing],
  declarations: [Pages],
  providers: [AppService, ConfigService, NotificationService]
})
export class PagesModule {
}
