import { html, TemplateResult, render } from "lit-html";
import { repeat } from "lit-html/lib/repeat";
import { unsafeHTML } from "../../../../node_modules/lit-html/lib/unsafe-html.js";

const styles = unsafeHTML(`<style>${require("./dots-button.component.css")}<style>`);

export class DotsButtonComponent extends HTMLElement {
    constructor() {
        super();
    }

    static get observedAttributes () {
        return [
            "size"
        ];
    }

    connectedCallback() {     

        this.attachShadow({ mode: 'open' });
        
        render(this.template, this.shadowRoot);

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'dotsbutton');        
    }

    get template(): TemplateResult {
        return html`
            ${styles}
            <style>
                :host {
                    --size:${this.size}px;
                }
            </style>
            <svg width="${this.size}" height="${this.size}" viewBox="0 0 24 24">
            <path id="dots" d="M16,12A2,2 0 0,1 18,10A2,2 0 0,1 20,12A2,2 0 0,1 18,14A2,2 0 0,1 16,12M10,12A2,2 0 0,1 12,10A2,2 0 0,1 14,12A2,2 0 0,1 12,14A2,2 0 0,1 10,12M4,12A2,2 0 0,1 6,10A2,2 0 0,1 8,12A2,2 0 0,1 6,14A2,2 0 0,1 4,12Z" />
            </svg>`;
    }

    public size: number = 24;
    
    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "size":
                this.size = Number(newValue);
                break;
        }
    }
}

customElements.define(`cs-dots-button`,DotsButtonComponent);