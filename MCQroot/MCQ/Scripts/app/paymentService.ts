module Mcq {
    export module Home {
        export function paymentServiceFactory($http: ng.IHttpService, $q: ng.IQService, helpers: Mcq.Helpers.HttpHelpers) {
            return new PaymentService($q, $http, helpers);
        }

        export class PaymentService {
            constructor(
                private $q: ng.IQService,
                private $http: ng.IHttpService,
                private helpers: Mcq.Helpers.HttpHelpers) {
            }

            getPaymentForCurrentUser(): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.get(window["urls"].services.quizService ) });
            }

            pay(paymentViewModel: PaymentViewModel): ng.IPromise<ResponseResult> {
                return this.helpers.http(() => { return this.$http.post(window["urls"].services.paymentService, paymentViewModel) });
            }
        }
    }
}

Mcq.Home.paymentServiceFactory.$inject = ["$http", "$q", "httpHelpers"];      