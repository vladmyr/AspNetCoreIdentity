import { Component, Inject } from "@angular/core";
import { Http } from "@angular/http";

interface ClaimsVM {
    type: string;
    value: string;
}

interface UserClaims {
    claims: ClaimsVM[];
    userName: string;
}

@Component({
    selector: "claims",
    templateUrl: "./claims.component.html",
    styleUrls: ["./claims.component.css"]
})

export class ClaimsComponent {
    public claims: ClaimsVM[] = [];
    public userName: string = "";

    public http: Http;
    public baseUrl: string;

    constructor(
        http: Http,
        @Inject("BASE_URL") baseUrl: string
    ) {
        this.http = http;
        this.baseUrl = baseUrl;

        this.http
            .get(this.baseUrl + "/api/account/claims")
            .subscribe(result => {
                const claimsResult: UserClaims = result.json();

                this.claims = claimsResult.claims;
                this.userName = claimsResult.userName;
            }, error => {
                console.log(error);
            });
    }
}