module Mcq {
    export module Home {
        export function accountServiceFactory($http: ng.IHttpService, $q: ng.IQService, helpers: Mcq.Helpers.HttpHelpers) {
            return new AccountService($q, $http, helpers);
        }

        export class AccountService {
            constructor(
                private $q: ng.IQService,
                private $http: ng.IHttpService,
                private helpers: Mcq.Helpers.HttpHelpers) {
            }
            login(loginViewModel: LoginViewModel): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.post(window["urls"].services.accountService + "/login", loginViewModel) }, false);
            }

            loginExternal(loginViewModel: LoginViewModel): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.post(window["urls"].services.accountService + "/logInExternal", loginViewModel) }, false);
            }

            register(resiterViewModel: RegisterViewModel): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.post(window["urls"].services.accountService + "/register", resiterViewModel) }, false);
            }

            logOff(): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.accountService + "/logOff") }, false);
            }

            getCurrentUser(): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.accountService + "/getCurrentUser") }, false);
            }

            getPaymentInfo(): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.accountService + "/getPaymentInfo") }, false);
            }

            resetPassword(email: string): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.post(window["urls"].services.accountService + "/resetPassword", { email: email }) }, false);
            }

            updatePassword(password: string): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.post(window["urls"].services.accountService + "/updatePassword", { password: password }) }, false);
            }
        }
    }
}

Mcq.Home.accountServiceFactory.$inject = ["$http", "$q", "httpHelpers"];   