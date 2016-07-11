module Mcq {
    export module Home {
        export function navLoginController($scope: IAccountInfoController, accountService: AccountService, $rootScope, $state) {

            $scope.logOff = () => {
                accountService.logOff().then(() => {
                    $rootScope.loggedUser = null;
                });
            }

            //$scope.$on("onUserLogged",  (a, b) => {
            //    $scope.$$phase || $scope.$apply();
            //});
        }
    }
}

Mcq.Home.navLoginController.$inject = ["$scope", "accountService", "$rootScope", "$state"]; 