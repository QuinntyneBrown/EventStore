import { html, TemplateResult, render } from "lit-html";
import { repeat } from "lit-html/lib/repeat";
import { unsafeHTML } from "lit-html/lib/unsafe-html";
import { constants } from "./auto-complete-events";

const styles = unsafeHTML(`<style>${require("./category.component.css")}<style>`);

export const CATEGORY_CLICKED = "[Category] Clicked";

export class CategoryComponent extends HTMLElement {
    constructor() {
        super();
        this.handleClick = this.handleClick.bind(this);
        this.refreshLinks = this.refreshLinks.bind(this);
    }

    public handleClick() {
        this.dispatchEvent(new CustomEvent(CATEGORY_CLICKED, {
            scoped: true,
            bubbles: true,
            composed: true,
            cancelable: true,
            detail: { text: this.innerText }
        } as CustomEventInit));
    }

    connectedCallback() {     

        this.attachShadow({ mode: 'open' });
        
		render(this.template, this.shadowRoot);

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'category');

        this._bind();
        this._setEventListeners();
    }

    get template(): TemplateResult {
        return html`
            ${styles}
            <slot></slot>
            <ul>
            ${repeat(this.links, i => i.caption, i => html`
            <li>${i.caption}</li>`)}
            </ul>
        `;
    }

    public refreshLinks(e: any) {
        
        for (let i = 0; i < e.detail.searchResultItems.length; i++) {
            if (this.innerText == e.detail.searchResultItems[i].caption) {
                this.links = e.detail.searchResultItems[i].children;
            }
        }

        render(this.template, this.shadowRoot);
    }

    private async _bind() {
        
    }

    private _setEventListeners() {
        this.addEventListener("click", this.handleClick);
        document.addEventListener(constants.SEARCH_RESULT_ITEMS_FETCHED, this.refreshLinks);
    }

    disconnectedCallback() {
        this.removeEventListener("click", this.handleClick);
        document.removeEventListener(constants.SEARCH_RESULT_ITEMS_FETCHED, this.refreshLinks);
    } 

    public links: Array<{ caption:string, url:string }> = [];
}

customElements.define(`cs-category`,CategoryComponent);
