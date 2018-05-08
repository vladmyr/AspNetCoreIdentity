import { Component, Inject } from "@angular/core";
import { Http } from "@angular/http";
import { Router } from "@angular/router";

interface RegisterVM {
    userName: string,
    email: string,
    password: string;
    confirmPassword: string;
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
    selector: 'register',
    templateUrl: "./register.component.html",
    styleUrls: ["./register.component.css"]
})
export class RegisterComponent {
    public errors: string = "";
    public user: RegisterVM = {
        userName: "",
        email: "",
        password: "",
        confirmPassword: ""
    }

    public http: Http;
    public baseUrl: string;
    public router: Router;

    public constructor(
        http: Http,
        @Inject("BASE_URL") baseUrl: string,
        router: Router
    ) {
        this.http = http;
        this.baseUrl = baseUrl;
        this.router = router;
    }

    public register() {
        this.errors = "";

        this.http
            .post(this.baseUrl + '/api/accout/register', this.user)
            .subscribe(result => {
                const registerResult: ResultVM = result.json();

                if (registerResult.status == StatusEnum.Error) {
                    this.errors = registerResult.data.toString();
                    return;
                }

                this.router.navigate(["/login"])
            }, error => {
                console.error(error);
            })
    }
}