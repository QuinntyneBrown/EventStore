import { render, html, TemplateResult } from "lit-html";
import { unsafeHTML } from "lit-html/lib/unsafe-html";

const styles = unsafeHTML(`<style>${require("./backdrop.component.css")}</style>`);

export class BackdropComponent extends HTMLElement {
    connectedCallback() {
        if(!this.shadowRoot) this.attachShadow({ mode: 'open' });

        render(html`${styles}`, this.shadowRoot);

        if (!this.hasAttribute('role'))
            this.setAttribute('role', 'backdrop');
    }
}

customElements.define(`cs-backdrop`, BackdropComponent);