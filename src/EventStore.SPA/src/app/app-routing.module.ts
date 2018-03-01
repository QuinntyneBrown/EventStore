import { Routes, RouterModule } from '@angular/router';
import { LoginModule } from "./login/login.module";
import { AppMasterPageComponent } from "./app-master-page.component";
import { AuthGuardService } from "./shared/services/auth-guard.service";
import { LoginMasterPageComponent } from "./login/login-master-page.component";
import { EventHubConnectionGuardService } from './shared/services/event-hub-connection-guard';

const canActivate = [
    AuthGuardService,
    EventHubConnectionGuardService
];

export const routes: Routes = [
    {
        path: 'login',
        component: LoginMasterPageComponent
    },
    {
        path: '',
        pathMatch:'full',
        canActivate,
        component: null,
        children: [
            {
                path: '',
                component: null
            }
        ]
    }
];

export const routing = RouterModule.forRoot(routes, { useHash: false });