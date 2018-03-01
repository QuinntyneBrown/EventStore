import {
    Component,
    Input,
    OnInit,
    EventEmitter,
    Output,
    AfterViewInit,
    AfterContentInit,
    Renderer,
    ElementRef,
} from "@angular/core";

import { FormGroup, FormControl, Validators } from "@angular/forms";

@Component({
    templateUrl: "./login.component.html",
    styleUrls: [
        "../shared/components/forms.css",
        "./login.component.css"
    ],
    selector: "ce-login"
})
export class LoginComponent implements AfterViewInit {

    constructor(private _renderer: Renderer, private _elementRef: ElementRef) {
        this.tryToLogin = new EventEmitter();
    }

    public get codeNativeElement(): HTMLElement {
        return this._elementRef.nativeElement.querySelector("#code");
    }

    ngAfterViewInit() {
        this._renderer.invokeElementMethod(this.codeNativeElement, 'focus', []);
    }

    ngAfterContentInit() {
        this.form.patchValue({
            code: this.code,
            password: this.password,
            rememberMe: this.rememberMe
        });
    }

    @Input()
    public code: string;

    @Input()
    public password: string;

    @Input()
    public rememberMe: boolean;

    @Output() public tryToLogin: EventEmitter<any>;

    public form = new FormGroup({
        code: new FormControl('', [Validators.required]),
        password: new FormControl('', [Validators.required]),
        rememberMe: new FormControl('', [])
    });
}
