import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { LoginModule } from "./login/login.module";
import { SharedModule } from "./shared/shared.module";

import "./rxjs-extensions";

import { AppComponent } from './app.component';
import { AppMasterPageComponent } from "./app-master-page.component";

import { routing } from "./app-routing.module";
import { constants } from "./shared/constants";

import { Store, StoreModule } from '@ngrx/store';


const declarations = [
    AppComponent,
    AppMasterPageComponent
];

const providers = [
    { provide: constants.BASE_URL, useValue: "" },
    { provide: constants.DEFAULT_PATH, useValue: "/" }
];

@NgModule({
    imports: [
        routing,
        BrowserModule,
        CommonModule,
        FormsModule,
        RouterModule,
        StoreModule.forRoot({ }),
        LoginModule,
        SharedModule
    ],
    providers,
    declarations,
    exports: [declarations],
    bootstrap: [AppComponent]
})
export class AppModule { }
