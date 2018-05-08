import { Component, Inject } from "@angular/core";
import { Http } from "@angular/http";
import { StateService, UserState } from "../../core/state.service";

@Component({
    selector: 'app',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    public http: Http;
    public baseUrl: string;

    constructor (
        http: Http, 
        stateService: StateService, 
        @Inject('BASE_URL') baseUrl: string
    ) {
        this.http = http;
        this.baseUrl = baseUrl;

        this.http
            .get(this.baseUrl + 'api/account/authenticated')
            .subscribe(result => {
                const state: UserState = result.json();
                stateService.setAuthentication(state);
            }, error => {
                console.log(error);
            });
    }
}
