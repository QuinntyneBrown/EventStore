import { constants } from "./auto-complete-events";
import { render, TemplateResult, html } from "lit-html";
import { repeat } from "lit-html/lib/repeat";
import { unsafeHTML } from "lit-html/lib/unsafe-html";

const styles = unsafeHTML(`<style>${require("./auto-complete.component.css")}</style>`);

export class AutoCompleteComponent extends HTMLElement {
    constructor() {
        super();          
        this.refreshSearchResultItems = this.refreshSearchResultItems.bind(this);   
    }

    public apiKey: string;

    private get _searchBoxHTMLElement() { return this.shadowRoot.querySelector("cs-search-box"); }

    private get _searchResultItemsElement() { return this.shadowRoot.querySelector("cs-search-result-items"); }

    static get observedAttributes() {
        return [];
    }

    public get template(): TemplateResult {
        return html`
            ${styles}
            <cs-search-box></cs-search-box>
            <cs-category>Estimating</cs-category>
            <cs-category>Detailing</cs-category>
            <cs-category>System</cs-category>
            <cs-category>Exit</cs-category>
        `;
    }
    connectedCallback() {     
        if (!this.shadowRoot) this.attachShadow({ mode: 'open' });

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'autocomplete');

        render(this.template, this.shadowRoot);

        this._setEventListeners();
        this._searchBoxHTMLElement.setAttribute("api-key", this.apiKey);
    }
    
    private _setEventListeners() {
        this._searchBoxHTMLElement.addEventListener(constants.SEARCH_RESULT_ITEMS_FETCHED, this.refreshSearchResultItems);
    }

    public refreshSearchResultItems(e: any) {        
        //this._searchResultItemsElement.setAttribute("search-result-items", JSON.stringify(e.detail.searchResultItems));
    }

    disconnectedCallback() {
        this._searchBoxHTMLElement.removeEventListener(constants.SEARCH_RESULT_ITEMS_FETCHED, this.refreshSearchResultItems);
    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            default:
                break;
        }
    }    
}

customElements.define(`cs-auto-complete`, AutoCompleteComponent);