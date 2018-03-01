import {constants} from "./auto-complete-events";
import {Observable} from "rxjs/Observable";
import {Subscription} from "rxjs/Subscription";
import 'rxjs/add/observable/fromEvent';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/observable/from';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/debounceTime';
import 'rxjs/add/operator/do';
import { render, TemplateResult, html } from "lit-html";
import { repeat } from "lit-html/lib/repeat";
import { unsafeHTML } from "lit-html/lib/unsafe-html";
import { Subject } from "rxjs/Subject";

const styles = unsafeHTML(`<style>${require("./search-box.component.css")}</style>`);

export interface ILink
{
    caption: string,
    url: string,
    children?: Array<ILink>
}

    
export class SearchBoxComponent extends HTMLElement {
    constructor() {
        super();
    }

    static get observedAttributes() {
        return [
        ];
    }

    public get template():TemplateResult {
        return html`
            ${styles}            
            <input type="text" placeholder="Search For..." />
            <cs-magnify></cs-magnify>
        `;
    }
    private get _inputHTMLElement() { return this.shadowRoot.querySelector("input"); }

    connectedCallback() {
        if (!this.shadowRoot) this.attachShadow({ mode: 'open' });

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'searchbox');

        render(this.template, this.shadowRoot);

        this._setEventListeners();
    }

    private _subscription: Subscription;

    private _setEventListeners() {

        Observable.fromEvent(this._inputHTMLElement, "keyup")
            .debounceTime(200)
            .map(x => this._inputHTMLElement.value)
            .map(inputValue => {
                const links: Array<ILink> = [];                
                this._links.forEach(topLevelLink => {
                    links.push({
                        caption: topLevelLink.caption,
                        url: topLevelLink.url,
                        children: topLevelLink.children.length > 0 ? topLevelLink.children.filter(child => child.caption.indexOf(inputValue) > -1):[]
                    })
                });
                return links;
            })
            .do((x) => this.dispatchEvent(new CustomEvent(constants.SEARCH_RESULT_ITEMS_FETCHED, {
                bubbles: true,
                composed: true,
                cancelable: false,
                detail: { searchResultItems: x }
            } as CustomEventInit)))
            .takeUntil(this.unsubscribe)
            .subscribe();

        setTimeout(() => {
            this.dispatchEvent(new CustomEvent(constants.SEARCH_RESULT_ITEMS_FETCHED, {
                bubbles: true,
                composed: true,
                cancelable: false,
                detail: { searchResultItems: this._links }
            } as CustomEventInit))
        }, 100);
    }

    public unsubscribe: Subject<void> = new Subject();

    disconnectedCallback() {
        this.unsubscribe.next();
    }

    private _timeoutId: any;

    private get _url(): string { return ``; }

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            default:
                break;
        }
    }  

    private _links: Array<ILink> =
        [
            {
                caption: "Estimating",
                url: "",
                children: [
                    { caption: "Profile", url: "" },
                    { caption: "Complete", url: "" },
                    { caption: "Hardware", url: "" },
                    { caption: "Doors", url: "" },
                    { caption: "Frames", url: "" }
                ]
            },
            {
                caption: "Detailing",
                url: "",
                children: [
                    { caption: "Profile", url: "" },
                    { caption: "Complete", url: "" },
                    { caption: "Hardware", url: "" },
                    { caption: "Doors", url: "" },
                    { caption: "Frames", url: "" }
            ]
            },
            {
                caption: "System",
                url: "",
                children: [
                    { caption: "Company", url: "" },
                    { caption: "Customer", url: "" },
                    { caption: "Division", url: "" },
                    { caption: "Supplier", url: "" },
                    { caption: "Tax", url: "" }
                ]
            },
            {
                caption: "Exit",
                url: "/login",
                children: []
            }
        ];
}

customElements.define(`cs-search-box`,SearchBoxComponent);