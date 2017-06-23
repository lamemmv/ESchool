import { NgModule } from '@angular/core';

import { routing } from './login.routing.module';
import { LoginComponent } from './login.component';
import { LoginService } from './login.service';

@NgModule({
    imports: [
        routing
    ],
    declarations: [
        LoginComponent
    ],
    providers: [
        LoginService
    ]
})
export class LoginModule {
}
