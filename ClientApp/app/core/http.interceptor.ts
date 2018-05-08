import { Injectable } from "@angular/core";
import { 
    Request, 
    XHRBackend, 
    RequestOptions, 
    Response, 
    Http, 
    RequestOptionsArgs, 
    Headers 
} from "@angular/http";
import { Observable } from "rxjs/Observable";
import "rxjs/add/operator/catch";
import "rxjs/add/operator/throw";
import { Router } from "@angular/router";
import { StateService } from "./state.service";

@Injectable()
export class HttpInterceptor extends Http {
    public stateService: StateService;
    public router: Router;

    public constructor(
        backend: XHRBackend, 
        defaultOptions: RequestOptions,
        stateService: StateService,
        router: Router
    ) {
        super(backend, defaultOptions);

        this.stateService = stateService;
        this.router = router;
    }

    public request(
        url: string | Request,
        options?: RequestOptionsArgs
    ): Observable<Response> {
        return super
            .request(url, options)
            .catch((error: Response) => {
                if (
                    (error.status == 401 || error.status == 403)
                    && (window.location.href.match(/\?/g) || []).length < 2
                ) {
                    this.stateService.setAuthentication({ 
                        userName: '',
                        isAuthenticated: false
                    });
                    this.router.navigate(['/login'])
                }
                return Observable.throw(error);
            });
    }
}