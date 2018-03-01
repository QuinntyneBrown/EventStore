import { Injectable, NgZone } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import "rxjs/add/observable/fromPromise";
import "rxjs/add/operator/map";
import { constants } from "../constants";
import { Storage } from "../services/storage.service";
import { Subject } from "rxjs/Subject";
import { HubConnection } from "@aspnet/signalr-client";

export function EventHubFactory(storage: Storage, ngZone: NgZone) {
    EventHub.instance = EventHub.instance || new EventHub(storage, ngZone);
    return EventHub.instance;
}

@Injectable()
export class EventHub {
    private _licenseHubProxy: any;
    private _connection: HubConnection;
    public events: Subject<any> = new Subject();
    public static instance;


    constructor(
        private _storage: Storage,
        private _ngZone: NgZone) {
    }

    private _connect: Promise<any>;
    
    public connect(): Promise<any> {    

        if (this._connect)
            return this._connect;
        
        this._connect = new Promise((resolve) => {

            const accessToken = this._storage.get({ name: constants.ACCESS_TOKEN_KEY });
            
            this._connection = this._connection || new HubConnection(`/events?token=${accessToken}`);
            
            this._connection.on("serverEvent", (value: any) => { 
                console.log(value);
                this._ngZone.run(() => this.events.next(value));                
            });

            this._connection.start().then(() => resolve());
        });

        return this._connect;
    }

    public disconnect() {
        if (this._connection) {
            this._connection.stop();
            this._connect = null;
        }
    }
}