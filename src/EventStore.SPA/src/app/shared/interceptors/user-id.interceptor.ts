import { Injectable } from "@angular/core";
import { Http, Headers, RequestOptionsArgs } from "@angular/http";
import { constants } from "../constants";
import { Storage } from "../services/storage.service";
import { Observable } from "rxjs";
import { HttpClient, HttpEvent, HttpInterceptor, HttpRequest, HttpHandler } from "@angular/common/http";

@Injectable()
export class UserIdInterceptor implements HttpInterceptor {
    constructor(private _storage: Storage) { }

    intercept(httpRequest: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(httpRequest.clone({
            headers: httpRequest.headers.set('UserId', `${this._storage.get({ name: constants.AUTHENTICATED_USER_ID_KEY })}`)
        }));
    }
}