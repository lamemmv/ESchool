import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './../shared/authentications/auth.service';
@Component({
    templateUrl: './login.component.html',
    styleUrls: [
        "./login.style.css"
    ],
    encapsulation: ViewEncapsulation.None
})
export class LoginComponent {
    message: string;
    constructor(public authService: AuthService, public router: Router) {
        this.setMessage();
    }
    setMessage() {
        this.message = 'Logged ' + (this.authService.isLoggedIn ? 'in' : 'out');
    }
    login() {
        this.message = 'Trying to log in ...';
        this.authService.login().subscribe(() => {
            this.setMessage();
            if (this.authService.isLoggedIn) {
                // Get the redirect URL from our auth service
                // If no redirect has been set, use the default
                let redirect = this.authService.redirectUrl ? this.authService.redirectUrl : '/admin';
                // Redirect the user
                this.router.navigate([redirect]);
            }
        });
    }
    logout() {
        this.authService.logout();
        this.setMessage();
    }
}
