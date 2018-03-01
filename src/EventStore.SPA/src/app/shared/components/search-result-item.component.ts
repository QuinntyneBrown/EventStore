import { constants } from "./auto-complete-events";
import { SearchResultItem } from "./auto-complete.interfaces";
import { render, TemplateResult, html } from "lit-html";
import { repeat } from "lit-html/lib/repeat";
import { unsafeHTML } from "lit-html/lib/unsafe-html";

const styles = unsafeHTML(`<style>${require("./search-result-item.component.css")}</style>`);

export class SearchResultItemComponent extends HTMLElement {
    constructor() {
        super();        
        this.dispatchSearchResultItemEvent = this.dispatchSearchResultItemEvent.bind(this); 
    }

    static get observedAttributes() {
        return [
            "search-result-item"
        ];
    }

    private get headingHTMLElement() { return this.shadowRoot.querySelector("h2"); }

    private get thumbnailHTMLElement() { return this.shadowRoot.querySelector("img"); }

    private get searchResultItemDetailsHTMLElement() { return this.shadowRoot.querySelector("cs-search-result-item-detail"); }

    public get template(): TemplateResult {
        return html`
            ${styles}
            <div class="search-result-item-container">
                <img />
                <div class="search-result-item-details">
                    <h2></h2>
                </div>
            </div>

            <cs-search-result-item-detail></cs-search-result-item-detail>
        `;
    }

    connectedCallback() {                
        if (!this.shadowRoot) this.attachShadow({ mode: 'open' });

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'searchresultitem');

        if (!this.hasAttribute('tabindex'))
            this.setAttribute('tabindex', '0');

        render(this.template, this.shadowRoot);

        this._bind();
        this._setEventListeners();
    }

    disconnectedCallback() {
        this.removeEventListener("click", this.dispatchSearchResultItemEvent);
    }

    private _bind() {
        if (this.searchResultItem) {
            this.headingHTMLElement.innerHTML = this.searchResultItem.name;
            this.thumbnailHTMLElement.src = this.searchResultItem.image_thumb_url == null ? this.defaultImageUrl : this.searchResultItem.image_thumb_url;
            this.searchResultItemDetailsHTMLElement.setAttribute("search-result-item", JSON.stringify(this.searchResultItem));
        }
    }

    public get defaultImageUrl() { return ""; }

    private _setEventListeners() {
        this.addEventListener("click", this.dispatchSearchResultItemEvent);
    }

    public dispatchSearchResultItemEvent () {
        this.dispatchEvent(new CustomEvent(constants.SEARCH_RESULT_ITEM_CLICKED, {
            bubbles: true,
            composed: true,
            cancelable: false,
            detail: { searchResultItem: this.searchResultItem }
        } as CustomEventInit));
    }

    public set activeSearchResultItem(value: SearchResultItem) {        
        if (this.searchResultItem.id == value.id && !this.classList.contains("active")) {         
            this.classList.add("active");
        } else {
            this.classList.remove("active")
        }
    }
    
    public searchResultItem: SearchResultItem;

    attributeChangedCallback(name, oldValue, newValue) {
        switch (name) {
            case "search-result-item":                
                this.searchResultItem = JSON.parse(newValue);

                if (this.parentNode)
                    this._bind();
                break;
        }
    }   
}

customElements.define(`cs-search-result-item`, SearchResultItemComponent);