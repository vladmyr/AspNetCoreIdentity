import { Injectable } from "@angular/core";

export interface UserState {
    userName: string;
    isAuthenticated: boolean;
}

@Injectable()
export class StateService {
    userState: UserState = { userName: '', isAuthenticated: false };

    constructor() {}

    public setAuthentication(state: UserState) {
        this.userState = state;
    }

    public isAuthenticated() {
        return this.userState.isAuthenticated;
    }
}