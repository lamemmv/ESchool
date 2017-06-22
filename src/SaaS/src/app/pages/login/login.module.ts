import { NgModule } from '@angular/core';

import { routing } from './login.routing.module';
import { LoginComponent } from './login.component';
@NgModule({
    imports: [
        routing
    ],
    declarations: [
        LoginComponent
    ],
    providers: [
    ]
})
export class LoginModule {
}
