import { Routes, RouterModule } from '@angular/router';
import { FooterComponent } from './footer.component';

const routes : Routes = [
    {
        path: '',
        component: FooterComponent
    }
]

export const routing = RouterModule.forChild(routes)