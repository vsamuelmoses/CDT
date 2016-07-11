module Mcq {
    export module Home {
        export function registerController($scope: IRegisterScope, accountService: AccountService, growl, $rootScope) {
            $scope.register = () => {
                if (!$scope.registerViewModel) {
                    growl.warning("Fields are empty");
                    return;
                }
                accountService.register($scope.registerViewModel).then((response) => {
                    if (!response.isSucceed) {
                        if (response.data.errors && response.data.errors.length > 0) {
                            growl.warning(Enumerable.from<any>(response.data.errors).where((x) => { return x.indexOf('Name') === -1 }).toArray().join(' '));
                        }

                        $scope.validationErrors = response.data;
                        $scope.$$phase || $scope.$apply();
                    } else {
                        response.data.userName = response.data.firstName + " " + response.data.lastName;
                        $rootScope.loggedUser = response.data;
                    }
                });
            }

        }
    }
}

Mcq.Home.registerController.$inject = ["$scope", "accountService", "growl", "$rootScope"];  