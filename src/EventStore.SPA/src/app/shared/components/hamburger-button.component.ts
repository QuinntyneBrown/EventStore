import { html, TemplateResult, render } from "lit-html";
import { repeat } from "lit-html/lib/repeat";
import { unsafeHTML } from "lit-html/lib/unsafe-html";

const styles = unsafeHTML(`<style>${require("./hamburger-button.component.css")}<style>`);

export class HamburgerButtonComponent extends HTMLElement {
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
            this.setAttribute('role', 'hamburgerbutton');
    }

    get template(): TemplateResult {
        return html`
            ${styles}
            <svg width="24" height="24" viewBox="0 0 24 24">
            <path id="path" d="M3,6H21V8H3V6M3,11H21V13H3V11M3,16H21V18H3V16Z" /></svg>
        `;
    }

    public size: number = 24;

    disconnectedCallback() {

    }

    attributeChangedCallback (name, oldValue, newValue) {
        switch (name) {
            case "size":
                this.size = Number(newValue);
                break;
        }
    }
}

customElements.define(`cs-hamburger-button`,HamburgerButtonComponent);
