module Mcq {
    export module Helpers {
        export function httpHelpersFactory($q: ng.IQService, growl, $rootScope) {
            return new HttpHelpers($q, growl, $rootScope);
        }

        export class HttpHelpers {
            constructor(
                private $q: ng.IQService,
                private growl: any,
                private $rootScope) {
            }

            http(delegate, shouldHandleOperationResult = false) {
                this.$rootScope.isAjax = true;

                var deferred = this.$q.defer();
                delegate()
                    .success((result) => {
                    if (shouldHandleOperationResult) {
                        var opResult = result;
                        if (opResult.isSucceed) {
                            this.growl.success("Done");
                        } 
                    }
                    deferred.resolve(result);
                    this.$rootScope.isAjax = false;
                })
                    .error((result) => {
                    if (result) {
                        this.growl.error("Unexpected server error");
                    }
                    deferred.reject(result);
                    this.$rootScope.isAjax = false;
                });
                return deferred.promise;
            }
        }
    }
}

Mcq.Helpers.httpHelpersFactory.$inject = ["$q", "growl", "$rootScope"];  