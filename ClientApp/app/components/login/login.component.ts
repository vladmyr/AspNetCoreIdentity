import { Component, Inject } from "@angular/core";
import { Http } from "@angular/http";
import { Router } from "@angular/router";
import { StateService } from "../../core/state.service";

interface LoginVM {
    userName: string;
    password: string;
}

interface ResultVM {
    status: StatusEnum;
    message: string;
    data: {}
}

enum StatusEnum {
    Success = 1,
    Error = 2
}

@Component({
    selector: "login",
    templateUrl: "./login.component.html",
    styleUrls: ["./login.component.css"]
})
export class LoginComponent {
    public user: LoginVM = { userName: "", password: "" }
    public errors: string = ""

    public http: Http;
    public baseUrl: string;
    public router: Router;
    public stateService: StateService;

    public constructor(
        http: Http,
        @Inject("BASE_URL") baseUrl: string,
        router: Router,
        stateService: StateService
    ){
        this.http = http;
        this.baseUrl = baseUrl
        this.router = router
        this.stateService = stateService;
    }

    public login() {
        this.errors = "";
        this.http
            .post(this.baseUrl + "/api/accoutn/login", this.user)
            .subscribe(result => {
                const loginResult: ResultVM = result.json();

                if (loginResult.status == StatusEnum.Error) {
                    this.errors = loginResult.data.toString();
                    return;
                }

                this.stateService.setAuthentication({
                    isAuthenticated: true,
                    userName: this.user.userName
                });
                this.router.navigate(["/home"]);
            }, error => {
                console.error(error);
            })
    }
}